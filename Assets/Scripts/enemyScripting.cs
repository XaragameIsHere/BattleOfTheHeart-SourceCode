using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;




public class enemyScripting : MonoBehaviour
{
    ParticleSystem.NoiseModule noice;
    ParticleSystem.ShapeModule shape;
    ParticleSystem.MainModule main;
    ParticleSystem.RotationOverLifetimeModule rotationOverLifetime;
    ParticleSystem.CollisionModule particleCollider;
    ParticleSystem.ForceOverLifetimeModule forceOverLifetime;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.SubEmittersModule explody;
    ParticleSystem.TextureSheetAnimationModule textureAnimation;
    ParticleSystemRenderer atkRenderer;
    [SerializeField] GameObject fallingObjectArea;
    [SerializeField] playerMovement playerScript;
    [SerializeField] playerUIController Controller;
    [SerializeField] AudioClip FightMusic;
    [SerializeField] AudioClip dialogueMusic;
    [SerializeField] LayerMask bulletLayer;
    [SerializeField] Canvas death;
    [SerializeField] GameObject bigGuySpawn;
    [SerializeField] GameObject bigGuy;
    [SerializeField] Vector3 playerPostion;
    [SerializeField] Vector3 enemyPosition;
    [SerializeField] tutorialScripting tutorial;
    [SerializeField] GameObject tutorialKey;
    [SerializeField] GameObject tutorialFlyingKey;
    [SerializeField] BoxCollider2D teleporter;
    [SerializeField] GameObject eKey;
    [SerializeField] GameObject dropTemplate;
    [HideInInspector] public dialogueParsing.Dialogue dialogueRoot;
    [HideInInspector] public bool hit = false;
    public GameObject enemyObject;
    [ColorUsage(true, true)]
    public Color someHDRColor;
    [ColorUsage(true, true)]
    private Color newHDRColor = Color.white;
    private Animator enemyAnimator;
    
    [SerializeField] Volume postProcess;
    ColorAdjustments colorAd;

    public TextAsset jsonFile;
    public int test;
    [HideInInspector] public int enemyMax;
    public int enemyHealth = 10;
    public ParticleSystem shooter;
    public BoxCollider2D boxTrigger;
    public PolygonCollider2D playerCollider;
    public GameObject player;
    private AudioSource audioSystem;
    private SpriteRenderer spriteComponent;
    private LineRenderer lasersBitch;
    private AudioSource audio;
    private RaycastHit2D snipeHit;

    [HideInInspector, SerializeField] public enum attacks
    {
        doubleShot,
        Throwable,
        spray,
        narrow,
        snipe,
        drop,
        dropInTheBigGuy,
        sloMoParry,
    }

    [System.Serializable]
    public struct attackData
    {
        public Sprite[] sprites;
        public attacks attackType;
        public bool parryable;
    }

    public List<attackData> availableAttackTypes = new List<attackData>();

    [HideInInspector] public attackData currentData;


    // Start is called before the first frame update
    void Start()
    {
        enemyMax = enemyHealth;
        postProcess.profile.TryGet(out colorAd);
        colorAd.colorFilter.hdr = false;
        shooter = GetComponent<ParticleSystem>();
        emission = shooter.emission;
        forceOverLifetime = shooter.forceOverLifetime;
        main = shooter.main;
        noice = shooter.noise;
        shape = shooter.shape;
        explody = shooter.subEmitters;
        rotationOverLifetime = shooter.rotationOverLifetime;
        textureAnimation = shooter.textureSheetAnimation;
        particleCollider = shooter.collision;
        atkRenderer = GetComponent<ParticleSystemRenderer>();
        shooter.Play();

        //currentData = availableAttackTypes[0];
        enemyAnimator = GetComponent<Animator>();
        lasersBitch = GetComponent<LineRenderer>();
        spriteComponent = GetComponent<SpriteRenderer>();
        audioSystem = GetComponent<AudioSource>();

        dialogueRoot = JsonUtility.FromJson<dialogueParsing.Dialogue>(jsonFile.text);
    }

    private void clearSprites()
    {
        print(textureAnimation.spriteCount);
        if (textureAnimation.GetSprite(0) is Sprite)
        {
            for (int i = textureAnimation.spriteCount; i >= 1; i--)
            {
                print(textureAnimation.GetSprite(i - 1).name);
                textureAnimation.RemoveSprite(i - 1);
            }
        }
        
    }

    private void regenerateSprites()
    {
        for (int i = 0; i < currentData.sprites.Length; i++)
        {
            textureAnimation.AddSprite(currentData.sprites[i]);
        }
    }

    IEnumerator slowMoParry()
    {
        clearSprites();
        regenerateSprites();

        yield return new WaitForSeconds(2);
        shape.rotation = new Vector3(0, -90, 0);
        shape.arc = 1;
        main.startSpeed = 30;
        emission.rateOverTime = .75f;
        shooter.Emit(1);
        yield return new WaitForSeconds(.5f);

        tutorial.dropHint(tutorialKey);

        colorAd.colorFilter.Override(someHDRColor);
        main.simulationSpeed = .08f;
        yield return new WaitUntil(() => hit == true);
        colorAd.colorFilter.Override(newHDRColor);
        main.simulationSpeed = .6f;

        loopFight();

    }

    public void initializeFight()
    {
        //player.GetComponent<AudioSource>().Stop();
        //audioSystem.Play();
        playerScript.inFight = true;
        playerScript.inDialogue = true;
        player.transform.DOLocalMove(playerPostion, .5f);
        Controller.startDialogue(dialogueRoot);
        currentData = availableAttackTypes[0];


    }

    public void endFight()
    {
        enemyAnimator.SetBool("stunned", false);
        teleporter.isTrigger = true;
    }
    
    IEnumerator drop()
    {
        List<GameObject> falling = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {

            
            Instantiate(dropTemplate);
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = currentData.sprites[Random.Range(0, currentData.sprites.Length)];

        }

        for (int v = 0; v < 4; v++)
        {

            foreach (GameObject fallingObject in falling)
            {
                fallingObject.transform.localPosition = fallingObjectArea.transform.GetChild(Mathf.RoundToInt(Random.Range(1, 11))).localPosition;
            }

            yield return new WaitForSeconds(3);

            enemyAnimator.SetTrigger("Throw");
            foreach (GameObject fallingObject in falling)
            {
                fallingObject.GetComponent<Rigidbody2D>().simulated = true;
            }

            yield return new WaitForSeconds(2f);

            foreach (GameObject fallingObject in falling)
            {
                fallingObject.GetComponent<Rigidbody2D>().simulated = false;
            }

        }
        loopFight();
    }

    private IEnumerator snipe()
    {
        

        lasersBitch.SetPosition(0, transform.position);
        particleCollider.lifetimeLoss = 1;
        for (int i = 0; i < 8; i++)
        {
            rotate = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(player.transform.position.y - transform.position.y) / Mathf.Abs(player.transform.position.x - transform.position.x));

            snipeHit = Physics2D.Raycast(transform.position, new Vector2(-rotate, 0));
            if (snipeHit.collider != null)
            {
                lasersBitch.enabled = true;
                DOTween.To(() => lasersBitch.GetPosition(1), (x) => lasersBitch.SetPosition(1, x), player.transform.position, .5f).Play();
                yield return new WaitForSeconds(1);

                shape.rotation = new Vector3(-rotate, -90, 0);
                shape.arc = 1;
                main.startSpeed = 50;
                emission.rateOverTime = .75f;
                enemyAnimator.SetTrigger("Snipe");
                shooter.Emit(1);
                yield return new WaitForSeconds(.5f);
                lasersBitch.enabled = false;

            }
        }


        loopFight();

    }

    float rotate;
    private IEnumerator doubleShot()
    {
        

        rotationOverLifetime.enabled = true;
        particleCollider.lifetimeLoss = 1;
        emission.rateOverTime = 0;
        main.startSpeed = 35;
        main.startSize = 2;
        shooter.Play();
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
        bursts[0].count = 6;
        bursts[0].repeatInterval = 0.1f;
        bursts[0].time = 1;
        bursts[0].cycleCount = 2;
        bursts[0].probability = 1;
        emission.SetBursts(bursts);
        shape.arc = 360;
        shape.angle = 15;
        shape.radius = 2;
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
            emitOverride.startLifetime = 10f;
        
        for (int i = 0; i < 12; i++)
        {
            enemyAnimator.SetTrigger("DualShot");
            rotate = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(player.transform.position.y - transform.position.y) / Mathf.Abs(player.transform.position.x - transform.position.x));
            shape.rotation = new Vector3(-rotate, -90, 0);
            shooter.Emit(emitOverride, 24);
            yield return new WaitForSeconds(1.5f);
        }
            
        emission.SetBursts(new ParticleSystem.Burst[0]);
        print(textureAnimation.spriteCount);
        
        rotationOverLifetime.enabled = false;
        print("sasdfs");
        loopFight();
    }

    private IEnumerator spray()
    {

        rotationOverLifetime.enabled = true;
        
        //main.duration = 10;
        shooter.Play();
        main.loop = true;
        transform.DORotate(new Vector3(0, 0, 0), .5f);
        main.startSpeed = 15;
        emission.rateOverTime = 8;
        shape.angle = 60;
        shape.arc = 360;
        noice.enabled = true;
        noice.frequency = 1.59f;
        noice.scrollSpeed = 1.6f;
        noice.strength = 0.09f;
        yield return new WaitForSeconds(Random.Range(10, 20));
        shooter.Stop();
        
        rotationOverLifetime.enabled = false;
        loopFight();
    }

    private IEnumerator narrow()
    {
        
        shooter.Play();
        int fruityLoops = Mathf.RoundToInt(Random.Range(3, 7));
        DOTween.To(() => shape.angle, x => shape.angle = x, 10, 2);
        transform.Rotate(new Vector3(0, 0, -35));
        yield return transform.DORotate(new Vector3(0, 0, 30), 2).SetLoops(fruityLoops, LoopType.Yoyo).WaitForCompletion();
        shooter.Stop();
        
        loopFight();
    }

    private IEnumerator throwable()
    {
        
        shape.rotation = new Vector3(-60, -90, 0);
        shape.arc = 1;
        emission.rateOverTime = .5f;
        main.simulationSpeed = .4f;
        particleCollider.lifetimeLoss = 0;
        forceOverLifetime.enabled = true;

        explody.SetSubEmitterEmitProbability(0, 1);
        for (int i = 0; i < 6; i++)
        {
            enemyAnimator.SetTrigger("Throw");
            main.startSpeed = Mathf.Clamp((player.transform.localPosition.y/12) * 140, 60, 120);
            shooter.Emit(1);
            yield return new WaitForSeconds(1.25f);
        }


        main.simulationSpeed = 1;
        forceOverLifetime.enabled = false;
        explody.SetSubEmitterEmitProbability(0, 0);
        
        loopFight();

    }


    IEnumerator dropInTheBigGuy()
    {
        var newBigGuy = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        var bigGuyRbody = gameObject.AddComponent<Rigidbody2D>();

        spriteRenderer.sprite = currentData.sprites[0];

        for (int v = 0; v < 8; v++)
        {
            yield return new WaitForSeconds(3);
            bigGuyRbody.AddForce(new Vector2(5000 * player.transform.position.x - newBigGuy.transform.position.x, 30000));
            print("bee");
        }

        bigGuyRbody.simulated = false;
        Tween tween = newBigGuy.transform.DOLocalMoveY(46, 2.5f);
        
        loopFight();
        yield return tween.WaitForCompletion();
        Destroy(newBigGuy);
    }

    IEnumerator waitForFall()
    {
        yield return new WaitForSeconds(1);
        playerScript.inFight = true;
        loopFight();
    }

    public bool FirstLevel;
    public void continualizeFight()
    {

        print("start");

        //audioSystem.clip = FightMusic;
        //audioSystem.Play();
        transform.DOLocalMove(enemyPosition, 1);
        playerScript.inFight = true;

        if (FirstLevel)
        {
            print("sdsf");
            tutorial.dropHint(tutorialFlyingKey);
            StartCoroutine(slowMoParry());
        }
        else
        {
            enemyAnimator.SetBool("stunned", false);
            playerScript.inFight = false;
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000, 1000));
            StartCoroutine(waitForFall());
            
        }
    }

    
    IEnumerator check()
    {
        enemyAnimator.SetBool("stunned", true);
        yield return new WaitForSeconds(10);
        if (!playerScript.inDialogue)
        {
            
            loopFight();
            enemyAnimator.SetBool("stunned", false);
        }

    }




    public void loopFight()
    {
        clearSprites();
        regenerateSprites();

        print(currentData.attackType);
        if (enemyHealth > 0)
        {
            switch (currentData.attackType)
            {
                case attacks.snipe:
                    StartCoroutine(snipe());
                    break;
                case attacks.Throwable:
                    StartCoroutine(throwable());
                    break;
                case attacks.doubleShot:
                    StartCoroutine(doubleShot());
                    break;
                case attacks.narrow:
                    StartCoroutine(narrow());
                    break;
                case attacks.spray:
                    StartCoroutine(spray());
                    break;
                case attacks.drop:
                    StartCoroutine(drop()); 
                    break;
                case attacks.dropInTheBigGuy:
                    StartCoroutine(dropInTheBigGuy());
                    break;

            }

            currentData = availableAttackTypes[Random.Range(0,availableAttackTypes.Count)];
        }
        else
        {
            if (FirstLevel)
                tutorial.dropHint(eKey);
            
            StartCoroutine(check());
        }
    }


}

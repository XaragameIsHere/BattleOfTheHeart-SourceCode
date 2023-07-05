using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using jsonUtilities;

[System.Serializable]
public class Employee
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string firstName;
    public string lastName;
}

[System.Serializable]
public class Employees
{
    //employees is case sensitive and must match the string "employees" in the JSON.
    public Employee[] employees;
}

public class enemyScripting : MonoBehaviour
{
    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().sprite = ;
        initializeFight();
    }

    private void initializeFight()
    {
        print(jsonFile.text);
        Employees employeesInJson = JsonUtility.FromJson<Employees>(jsonFile.text);
        /*
        foreach (Employee employee in employeesInJson.employees)
        {
            Debug.Log("Found employee: " + employee.firstName + " " + employee.lastName);
        } */
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

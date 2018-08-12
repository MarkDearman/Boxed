using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    bool pauseSim = false;

    public int level = 0;

    public int CustomersSpawned = 0;

    public int[] LevelCustomers;

    public int Upsetcustom = 0;
    public int UpsetcustomMax = 0;
    public int HappyCustom = 0;
    public int MaxSpace = 30;
    public int SpaceLeft = 0;

    public TextMesh ScoreText;
    public int score;

    public Button DropButton;
    public Transform SelectedObj;
    public Player player;
    public float MaxPickupDistance = 1f;

    public Transform CustomerPrefab;
    public Transform CustomerSpawnPoint;
    public Transform exitpoint;
    public GameObject[] BoxPrefabs;
    public Transform[] BoxSpawnPoints;

    public TextMesh shipmentTimerSeconds;
    public TextMesh shipmentTimerMS;

    public TextMesh UC;
    public TextMesh HC;
    public TextMesh SpaceText;
    public TextMesh LevelText;

    public queuepoint[] Queue;

    public GameObject NextLevelBox;


    public float ShipmentTimeNormal;
    public float ShipmentTime;

    public Animator door;

    public List<Transform> customers = new List<Transform>();
    public List<Transform> boxes = new List<Transform>();

    public Renderer Green;
    public Renderer Amber;
    public Renderer Red;

    public TextMesh HighScore;

    

    private void Start()
    {
        ShipmentTime = ShipmentTimeNormal;
        OpenBoxDoor();
        InitLevel();
    }

    private void Update()
    {

        LevelText.text = (level + 1).ToString();
        
        HighScore.text = PlayerPrefs.GetInt("highscore").ToString();

        UC.text = Upsetcustom.ToString() + " / " + UpsetcustomMax.ToString();
        HC.text = HappyCustom.ToString();
        SpaceText.text = (SpaceLeft + " / " + MaxSpace);

        ScoreText.text = score.ToString();

        if(Upsetcustom >= UpsetcustomMax)
        {
            Failure();
        }

        if(SpaceLeft > MaxSpace)
        {
            Failure();
        }

        //if(customers.Count < Queue.Length)
        //{
        //    SpawnCustomer();
        //}

        if (player.ItemHeld)
        {
            //DropButton.interactable = true;
        }
        else
        {
            //DropButton.interactable = false;
        }

        if (!pauseSim)
        {

            ShipmentTime -= Time.deltaTime;

            float minutes = Mathf.FloorToInt(ShipmentTime / 60f);
            float seconds = Mathf.FloorToInt(ShipmentTime - minutes * 60);
            float fraction = Mathf.Round(ShipmentTime * 1000) % 100;

            shipmentTimerMS.text = fraction.ToString();
            shipmentTimerSeconds.text = seconds.ToString();

            if (ShipmentTime >= ShipmentTimeNormal / 2)
            {
                Green.material.color = Color.green;
                Amber.material.color = Color.black;
                Red.material.color = Color.black;
            }

            if (ShipmentTime < ShipmentTimeNormal / 2 && ShipmentTime >= ShipmentTimeNormal / 3)
            {
                Green.material.color = Color.black;
                Amber.material.color = Color.yellow;
                Red.material.color = Color.black;
            }

            if (ShipmentTime < ShipmentTimeNormal / 3)
            {
                Green.material.color = Color.black;
                Amber.material.color = Color.black;
                Red.material.color = Color.red;
            }

            if (ShipmentTime <= 0f)
            {
                ShipmentTime = ShipmentTimeNormal;
                OpenBoxDoor();
            }
        }

    }

    public void Failure()
    {
        PlayerPrefs.SetInt("currentScore", score);
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
        }

        SceneManager.LoadScene(2);
    }

    public void SpawnCustomer()
    {
        Transform Cus = Instantiate(CustomerPrefab, CustomerSpawnPoint.transform.position, CustomerSpawnPoint.transform.rotation);
        CustomersSpawned++;
        customers.Add(Cus);
    }

    public void DropObject()
    {
        player.DropObject();
    }

    public void ThrowBox()
    {
        for (int i = 0; i < BoxSpawnPoints.Length; i++)
        {
            GameObject Box = Instantiate(BoxPrefabs[Random.Range(0 ,BoxPrefabs.Length)], BoxSpawnPoints[i].transform.position, BoxSpawnPoints[i].transform.rotation);
            Box.GetComponent<Rigidbody>().AddForce(transform.right * 450);
            SpaceLeft++;
            score += 2;
            boxes.Add(Box.transform);
        }
        for (int x = 0; x < customers.Count; x++)
        {
            customers[x].GetComponent<customer>().Sweat();
        }
    }

    public void OpenBoxDoor()
    {
        door.Play("DoorCycle");
    }

    public void InitLevel()
    {
        CustomersSpawned = 0;
        NextLevelBox.SetActive(false);
        bool CusSpawned = false;

        if (CusSpawned == false)
        {
            int spawnedCus = 0;
            for (int i = 0; i < 12; i++)
            {
                SpawnCustomer();
                spawnedCus++;
            }

            if (spawnedCus == 12)
            {
                CusSpawned = true;
            }
        }
    }

    public void FinishLevel()
    {
        pauseSim = true;
        NextLevelBox.SetActive(true);
    }

    public void NextLevel()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            Destroy(boxes[i].gameObject);
        }

        SpaceLeft = 0;
        Upsetcustom = 0;

        if(level == LevelCustomers[LevelCustomers.Length - 1])
        {
            SceneManager.LoadScene(3);
        }
        pauseSim = false;
        NextLevelBox.SetActive(false);
        level++;
        InitLevel();
        ShipmentTimeNormal = ShipmentTimeNormal - 1;
        ShipmentTime = ShipmentTimeNormal;
    }

}

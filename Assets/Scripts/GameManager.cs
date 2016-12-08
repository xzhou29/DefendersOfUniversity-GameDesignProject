using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Element { STORM, FROST, NONE, BOSS, FIRE, ARCHITECTURE, AIROBOT, DEV } // Fire = CS  Frost = Business Storm = BookTower



public delegate void CurrencyChanged();
public class GameManager : Singleton<GameManager>
{

    #region EVENTS

    public event CurrencyChanged Changed;

    #endregion

    #region VARIABLES



    private Tower selectedTower;

    public int waveCount = 0;

    private int currency;

    private int lives; // play's lives

    public int wave = 0;

    private int health = 20;

    private int tmpHealth = 0;

    private bool gameOver;

    private float time;
    private float loadingTime;
    private float interestTimer = 20;

    private string nextWaveType = "";

    private string HintInvisibleWave = "Next wave hint: Loner is invisible, build Computer Architecture to detect them";
    private string HintWalkingDeadWave = "Next wave hint: Frat could be walking dead(Two lives)";
    private string HintinvulnerableWave = "Next wave hint: Hipster has a shield every 3 seconds";
    private string HintBrianWave = "Next wave hint: Brian is a boss!";
    private string HintHellfortWave = "Next wave hint: Hellford is coming!";
    private string HintFastSpeedWave = "Next wave hint:  Athlete runs faster.";
    private string HintDoubleHealthWave = "Next wave hint: Nerdy Girl has double health.";
    private string HintChangYunWave = "Win? ........... Not Yet";
    //  active monsters
    private List<Monster> activeMonsters = new List<Monster>();

    //ll the tower sprites
    [SerializeField]
    private Sprite[] towerSprites;

    [SerializeField]
    private GameObject pauseMenu;


    //[SerializeField]
    //private Image content;

    //[SerializeField]
    //private GameObject playButton;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private GameObject quitApplicationPanel;

    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject WinButton;

    [SerializeField]
    private GameObject waveBtn;


    [SerializeField]
    private Text upgradePrice;

    [SerializeField]
    private Text livesText;

    [SerializeField]
    private Text waveText;

    [SerializeField]
    private Text currencyText;

    [SerializeField]
    private Text sellText;

    [SerializeField]
    private Text stats;

    [SerializeField]
    private Text sizeText;

    [SerializeField]
    private Text InterestTimerText;

    [SerializeField]
    private Text textComp;


    private int towerCost = 0;

    #endregion

    #region PROPERTIES


    public ObjectPool Pool { get; private set; }

    // a property for the towerBtn
    public TowerBtn ClickedBtn { get; set; }

    #endregion
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {

            this.lives = value;


            if (lives <= 0)
            {
                lives = 0;
                GameOver();
            }

            livesText.text = lives.ToString();
        }
    }

    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value;

            this.currencyText.text =  value.ToString(); // " ";

            OnCurrencyChanged();
        }
    }

    public Text Stats
    {
        get
        {
            return stats;
        }
    }
    public Text SizeText
    {
        get
        {
            return sizeText;
        }
    }

    private void Awake()
    {
        //Instantiates the object pool
        Pool = GetComponent<ObjectPool>();
    }

    //private float loadingStatus = 0;


    private void Start()
    {
        // loadingTime += Time.deltaTime;

        //if (loadingTime > 0.5f)
        //{
        //    loadingStatus += 0.1f;
        //    content.transform.localScale = new Vector2(loadingStatus, content.transform.localScale.y);
        //} 

        Lives = 25;
        Currency = 2000;
        time = 0;
   
    }
    public void QuitApplication()
    {
        Application.Quit();
    }

    private void Update()
    {
       
        waveText.text =  "Level "+ (waveCount - 1).ToString(); //"<color=green> </color> " +
        InterestTimerText.text = "Interest in " + Mathf.Round(interestTimer);
        if (currency < towerCost)
        {
            Hover.Instance.Deactivate();
            ClickedBtn = null;
        }


        if (Input.GetMouseButton(1))
        {
            Hover.Instance.Deactivate();
            ClickedBtn = null;
        }
        if (selectedTower)
        {
            time += Time.deltaTime;
            if (time > 0.5f )
            {
                upgradePanel.SetActive(true);
            }

        }
        if (activeMonsters.Count > 0 && !gameOver)
            if (waveCount != 0 )
                interestTimer -= Time.deltaTime;
        gainInterest();

        //while (ClickedBtn)
        //{
        //    towerPanel.SetActive(false);
        //    waveBtn.SetActive(false);
        //    quitApplicationPanel.SetActive(false);
        //} 

        //if (upgradePanel)
        //{
        //    upgradePanel.transform.position = new Vector2(329f, 175f);

        //}


        //if (Input.GetKeyDown(KeyCode.Escape))//If we press the escape button
        //{

        //    quitApplicationPanel.SetActive(true);


        //    ////If we haven't selected a tower, then we need to show the ingame menu
        //    //if (selectedTower == null && !Hover.Instance.Visible)
        //    //{
        //    //    //Shows the menu
        //    //    MenuManager.Instance.ShowIngameMenu();
        //    //}
        //    //else if (Hover.Instance.Visible)//If we are holding a tower in our hand, then we drop it
        //    //{
        //    //    //Drops the tower
        //    //    DropTower();
        //    //}
        //    //else if (selectedTower != null) //If we have selected a tower, then we deselect it
        //    //{
        //    //    //Deselect the tower
        //    //    DeselectTower();
        //    //}
        //}
    }

    // When the currency changes
    public void OnCurrencyChanged()
    {
        if (Changed != null)
        {

            Changed();
        }
    }

    public void PickTower(TowerBtn towerBtn)
    {
        towerCost = towerBtn.Price;
        //If we have enough currency and we aren't in a wave phase
        if (Currency >= towerBtn.Price && activeMonsters.Count <= 0)
        {

            ClickedBtn = towerBtn;
            Hover.Instance.Activate(ClickedBtn.TowerSprite);
        }
    }


    private void DropTower()
    {

        ClickedBtn = null;
    }

    public void BuyTower()
    {
        //If we have enough currency to buy the tower
        if (Currency >= ClickedBtn.Price && Input.GetKey("left ctrl"))
        {

            Currency -= ClickedBtn.Price;
        }
        else if (Currency >= ClickedBtn.Price)
        {

            Currency -= ClickedBtn.Price;
            ClickedBtn = null;
            Hover.Instance.Deactivate();
        }

    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        sellText.text = "Sell +$" + (selectedTower.Price / 2).ToString() + " ";

      
     
        if (tower.NextUpgrade != null)
        {
            upgradePrice.text = "Upgrade -$" + tower.NextUpgrade.Price.ToString() + " ";
        }

        selectedTower.Select();
    }

    public void UpdateTooltip()
    {
        //If we have selected a tower
        if (selectedTower != null)
        {

            sellText.text = "<b>Sell</b> +$" + (selectedTower.Price / 2).ToString() + "";
            SetTooltipText(selectedTower.GetStats());

            if (selectedTower.NextUpgrade != null)//If  we have a upgrade, then we need to show the upgrade
            {
                upgradePrice.text = "<b>Upgrade</b> -$"+selectedTower.NextUpgrade.Price.ToString() + "";
            }
            else
            {
                upgradePrice.text = string.Empty;
            }

        }
    }


    public void SetTooltipText(string txt)
    {
        stats.text = txt; //Sets the stats text
        sizeText.text = txt; //sets the size text

    }

    public void DeselectTower()
    {

        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        upgradePanel.SetActive(false);
        //Remove the reference to the tower
        selectedTower = null;
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.Price / 2;
            //Gets the tile of the tower and makes it walkable again

            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;

            Destroy(selectedTower.transform.parent.gameObject);
            DeselectTower();

        }
    }

    

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }

        }
    }

    public void ShowStats()
    {
        //Show the stats panel
        statsPanel.SetActive(!statsPanel.activeSelf);
    }



    public void StartWave()
    {

        waveCount++;
        StartCoroutine(SpawnWave());
        ClickedBtn = null;
        Hover.Instance.Deactivate(); 
        waveBtn.SetActive(false);
        if(waveCount == 21)
        {
            WinButton.SetActive(true);
        }


    }
    
    
    public void GameOver()
    {

        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }

    }
   

    private IEnumerator SpawnWave()
    {
        //Generates the path
        LevelManager.Instance.GeneratePath();
       
        if (waveCount <= 9)
        {
            for (int i = 0; i < 14; i++)
            {
                //int monsterIndex = UnityEngine.Random.Range(0, 4);
                string type = string.Empty;
                if (waveCount == 1)
                {
                    type = "Basic";
                }           
                if (waveCount == 2)
                {
                    type = "Basic";
                    nextWaveType = "HintWalkingDeadWave";
                }
                    
                if (waveCount == 3)
                {
                    type = "Frat";
                    nextWaveType = "HintDoubleHealthWave";
                }    
                if (waveCount == 4)
                {
                    type = "NerdyGirl";
                    nextWaveType = "HintInvisibleWave";
                }
                    
                if (waveCount == 5)
                {
                    type = "Loner";
                    nextWaveType = "HintFastSpeedWave";
                }
                    
                if (waveCount == 6)
                {
                    type = "Athelete";
                    nextWaveType = "HintinvulnerableWave";
                }    
                if (waveCount == 7)
                {
                    type = "Hispter";
                    nextWaveType = "HintWalkingDeadWave";
                }          
                if (waveCount == 8)
                {
                    type = "Frat";
                    nextWaveType = "HintInvisibleWave";
                }
                   
                if (waveCount == 9)
                {
                    type = "Loner";
                    nextWaveType = "HintBrianWave";
                }
 
                Monster monster = Pool.GetObject(type).GetComponent<Monster>();
                if(type == "NerdyGirl" && waveCount > 3)
                {
                    tmpHealth = health * 2;
                    monster.Spawn(tmpHealth);
                }
                else
                    monster.Spawn(health);


                //Adds the monster to the activemonster list
                activeMonsters.Add(monster);
                yield return new WaitForSeconds(0.6f);
            }

        }
        

        else if (waveCount == 10)
        {
            Monster monster = Pool.GetObject("Boss_01").GetComponent<Monster>();
            monster.Spawn(1000);
            activeMonsters.Add(monster);
            nextWaveType = "HintFastSpeedWave";
        }
        else if ( (waveCount >= 11 && waveCount <=14) || (waveCount >= 16 && waveCount <= 19))
        {
            for (int i = 0; i < 14; i++)
            {
                //int monsterIndex = UnityEngine.Random.Range(0, 4);
                string type = string.Empty;
                if (waveCount == 11)
                {
                    type = "Athelete";
                    nextWaveType = "HintinvulnerableWave";
                } 
                if (waveCount == 12)
                {
                    type = "Hispter";
                    nextWaveType = "HintWalkingDeadWave";
                }
                    
                if (waveCount == 13)
                {
                    type = "Frat";
                    nextWaveType = "HintInvisibleWave";
                }  
                if (waveCount == 14)
                {
                    type = "Loner";
                    nextWaveType = "HintHellfortWave";
                }
                if (waveCount == 16)
                {
                    type = "Athelete";
                    nextWaveType = "HintinvulnerableWave";
                }
                if (waveCount == 17)
                {
                    type = "Hispter";
                    nextWaveType = "HintWalkingDeadWave";
                }

                if (waveCount == 18)
                {
                    type = "Frat";
                    nextWaveType = "HintInvisibleWave";
                }
                if (waveCount == 19)
                {
                    type = "Loner";
                    nextWaveType = "HintChangYunWave";
                }

                Monster monster = Pool.GetObject(type).GetComponent<Monster>();
                if (type == "NerdyGirl" && waveCount > 3)
                {
                    tmpHealth = health * 2;
                    monster.Spawn(tmpHealth);
                }
                else
                    monster.Spawn(health);
                //Adds the monster to the activemonster list
                activeMonsters.Add(monster);
                yield return new WaitForSeconds(0.6f);
            }

        }
        else if (waveCount == 15)
        {
            Monster monster = Pool.GetObject("Hellford").GetComponent<Monster>();
            monster.Spawn(2000);
            activeMonsters.Add(monster);
            nextWaveType = "HintFastSpeedWave";
        }
        else if (waveCount == 20)
        {
            Monster monster = Pool.GetObject("ChangYun").GetComponent<Monster>();
            monster.Spawn(4000);
            activeMonsters.Add(monster);
        }

         health += 25; 

    }


    private IEnumerator NextWaveHints()
    {
        textComp.enabled = true;

        if (nextWaveType == "HintInvisibleWave")
        {
            //yield return new WaitForSeconds(5f);
            while (textComp.enabled)
            {
                for (int i = 0; i <= HintInvisibleWave.Length; i++)
                {
                    textComp.text = HintInvisibleWave.Substring(0, i);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }
        else if (nextWaveType == "HintWalkingDeadWave")
        {
            //yield return new WaitForSeconds(5f);

            while (textComp.enabled)
            {
                for (int i = 0; i <= HintWalkingDeadWave.Length; i++)
                {
                    textComp.text = HintWalkingDeadWave.Substring(0, i);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }
        else if (nextWaveType == "HintDoubleHealthWave")
        {
            //yield return new WaitForSeconds(5f);

            while (textComp.enabled)
            {
                for (int i = 0; i <= HintDoubleHealthWave.Length; i++)
                {
                    textComp.text = HintDoubleHealthWave.Substring(0, i);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }
        else if (nextWaveType == "HintinvulnerableWave")
        {
            //yield return new WaitForSeconds(5f);

            while (textComp.enabled)
            {
                for (int i = 0; i <= HintinvulnerableWave.Length; i++)
                {
                    textComp.text = HintinvulnerableWave.Substring(0, i);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }
        else if (nextWaveType == "HintBrianWave")
        {
            //yield return new WaitForSeconds(5f);

            while (textComp.enabled)
            {
                for (int i = 0; i <= HintBrianWave.Length; i++)
                {
                    textComp.text = HintBrianWave.Substring(0, i);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }
        else if (nextWaveType == "HintFastSpeedWave")
        {
            //yield return new WaitForSeconds(5f);

            while (textComp.enabled)
            {
                for (int i = 0; i <= HintFastSpeedWave.Length; i++)
                {
                    textComp.text = HintFastSpeedWave.Substring(0, i);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }
        else if (nextWaveType == "HintHellfortWave")
        {
           // yield return new WaitForSeconds(5f);

            while (textComp.enabled)
            {
                for (int i = 0; i <= HintHellfortWave.Length; i++)
                {
                    textComp.text = HintHellfortWave.Substring(0, i);

                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }
        else if (nextWaveType == "HintChangYunWave") 
        {
            // yield return new WaitForSeconds(5f);

            while (textComp.enabled)
            {
                for (int i = 0; i <= HintChangYunWave.Length; i++)
                {
                    textComp.text = HintChangYunWave.Substring(0, i);

                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(5f);
                textComp.enabled = false;
            }
        }

        Debug.Log("Next wave hint displayed:");
    }

    public void Restart()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

  
    public void RemoveMonster(Monster monster)
    {
        //Removes the monster from the active list
        activeMonsters.Remove(monster);

        if (activeMonsters.Count <= 0 && !gameOver)
        {
            waveBtn.SetActive(true);
            Debug.Log("Next wave hint:");
            StartCoroutine(NextWaveHints());
            
        }
        if(waveCount > 20 && activeMonsters.Count <= 0)
        {
            WinButton.SetActive(true);
        }
        
    }

    private void gainInterest()
    {

        double interest;
        if (interestTimer <= 0)
        {
            interest = Currency * .1;
            Currency = Currency + (int) interest;
            interestTimer = 20;
        }
    }

    public void QuitToMenu()
    {

        SceneManager.LoadScene("menu", LoadSceneMode.Single);

    }



}
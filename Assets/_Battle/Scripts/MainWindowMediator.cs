using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScripts
{
    internal class MainWindowMediator : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] private TMP_Text _countMoneyText;
        [SerializeField] private TMP_Text _countHealthText;
        [SerializeField] private TMP_Text _countPowerText;
        [SerializeField] private TMP_Text _countCrimeText;

        [Header("Enemy Stats")]
        [SerializeField] private TMP_Text _countPowerEnemyText;

        [Header("Money Buttons")]
        [SerializeField] private Button _addMoneyButton;
        [SerializeField] private Button _minusMoneyButton;

        [Header("Health Buttons")]
        [SerializeField] private Button _addHealthButton;
        [SerializeField] private Button _minusHealthButton;

        [Header("Power Buttons")]
        [SerializeField] private Button _addPowerButton;
        [SerializeField] private Button _minusPowerButton;

        [Header("Crime Buttons")]
        [SerializeField] private Button _addCrimeButton;
        [SerializeField] private Button _minusCrimeButton;

        [Header("Other Buttons")]
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _passFightButton;

        private PlayerData _money;
        private PlayerData _heath;
        private PlayerData _power;
        private PlayerData _crime;

        private Enemy _enemy;
        private bool isPassed;


        private void Start()
        {
            _enemy = new Enemy("Enemy Flappy");

            _money = CreatePlayerData(DataType.Money);
            _heath = CreatePlayerData(DataType.Health);
            _power = CreatePlayerData(DataType.Power);
            _crime = CreatePlayerData(DataType.Crime);

            Subscribe();
        }

        private void OnDestroy()
        {
            DisposePlayerData(ref _money);
            DisposePlayerData(ref _heath);
            DisposePlayerData(ref _power);
            DisposePlayerData(ref _crime);

            Unsubscribe();
        }


        private PlayerData CreatePlayerData(DataType dataType)
        {
            PlayerData playerData = new PlayerData(dataType);
            playerData.Attach(_enemy);

            return playerData;
        }

        private void DisposePlayerData(ref PlayerData playerData)
        {
            playerData.Detach(_enemy);
            playerData = null;
        }


        private void Subscribe()
        {
            _addMoneyButton.onClick.AddListener(IncreaseMoney);
            _minusMoneyButton.onClick.AddListener(DecreaseMoney);

            _addHealthButton.onClick.AddListener(IncreaseHealth);
            _minusHealthButton.onClick.AddListener(DecreaseHealth);

            _addPowerButton.onClick.AddListener(IncreasePower);
            _minusPowerButton.onClick.AddListener(DecreasePower);

            _addCrimeButton.onClick.AddListener(IncreaseCrime);
            _minusCrimeButton.onClick.AddListener(DecreaseCrime);

            _fightButton.onClick.AddListener(Fight);
            _passFightButton.onClick.AddListener(PassFight);
        }

        private void Unsubscribe()
        {
            _addMoneyButton.onClick.RemoveAllListeners();
            _minusMoneyButton.onClick.RemoveAllListeners();

            _addHealthButton.onClick.RemoveAllListeners();
            _minusHealthButton.onClick.RemoveAllListeners();

            _addPowerButton.onClick.RemoveAllListeners();
            _minusPowerButton.onClick.RemoveAllListeners();

            _addCrimeButton.onClick.RemoveAllListeners();
            _minusCrimeButton.onClick.RemoveAllListeners();

            _fightButton.onClick.RemoveAllListeners();
            _passFightButton.onClick.RemoveAllListeners();
        }


        private void IncreaseMoney() => IncreaseValue(_money);
        private void DecreaseMoney() => DecreaseValue(_money);

        private void IncreaseHealth() => IncreaseValue(_heath);
        private void DecreaseHealth() => DecreaseValue(_heath);

        private void IncreasePower() => IncreaseValue(_power);
        private void DecreasePower() => DecreaseValue(_power);

        private void IncreaseCrime() => IncreaseValue(_crime);
        private void DecreaseCrime() => DecreaseValue(_crime);

        private void IncreaseValue(PlayerData playerData) => AddToValue(1, playerData);
        private void DecreaseValue(PlayerData playerData) => AddToValue(-1, playerData);

        private void AddToValue(int addition, PlayerData playerData)
        {
            playerData.Value += addition;
            ChangeDataWindow(playerData);
        }


        private void ChangeDataWindow(PlayerData playerData)
        {
            int value = playerData.Value;
            DataType dataType = playerData.DataType;
            TMP_Text textComponent = GetTextComponent(dataType);
            textComponent.text = $"Player {dataType:F} {value}";

            int enemyPower = _enemy.CalcPower();
            _countPowerEnemyText.text = $"Enemy Power {enemyPower}";
        }

        private TMP_Text GetTextComponent(DataType dataType) =>
            dataType switch
            {
                DataType.Money => _countMoneyText,
                DataType.Health => _countHealthText,
                DataType.Power => _countPowerText,
                DataType.Crime => _countCrimeText,
                _ => throw new ArgumentException($"Wrong {nameof(DataType)}")
            };


        private void PassFight()
        {   isPassed = true; 
            _fightButton.gameObject.SetActive(false);
            _passFightButton.gameObject.SetActive(false); 
            string color = "#07FF00";
            string message = "Passed";
            ShowMessages(color, message);
        }

        private void Fight()
        {
            int enemyPower = _enemy.CalcPower();
            bool isVictory = _power.Value >= enemyPower;

            string color = isVictory ? "#07FF00" : "#FF0000";
            string message = isVictory ? "Win" : "Lose";
            ShowMessages(color, message);
        }

     

        private void Update()
        {
            ShowPassFightAndFightButtons();

        }


        private void ShowPassFightAndFightButtons()
        {
            if (!CheckCrime() && !isPassed)
            {
                _passFightButton.gameObject.SetActive(true);
            }

            else if(isPassed)
            {
                _passFightButton.gameObject.SetActive(false);
                _fightButton.gameObject.SetActive(false);
            }

            else
            {
                _passFightButton.gameObject.SetActive(false);
                _fightButton.gameObject.SetActive(true);
            }
        }

     
        private bool CheckCrime()
        {
            if (_crime.Value > 3)
                return true;
            else
                return false;
        }

        private void ShowMessages(string color, string message)
        {
            Debug.Log($"<color={color}>{message}!!!</color>");
        }
    }
}

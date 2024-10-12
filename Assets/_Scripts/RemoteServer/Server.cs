using PKFTestAssignment.Enums;
using PKFTestAssignment.RemoteServer.Commands;
using PKFTestAssignment.RemoteServer.Simulation;
using PKFTestAssignment.World.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PKFTestAssignment.RemoteServer
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private CharacterData _playerData;
        [SerializeField] private CharacterData _swordsmanData;

        private readonly Dictionary<int, Adapter> _connectedClients = new();
        private readonly CombatSimulator _combatSimulator = new();
        private readonly CombatOutcomeSimulator _outcomeSimulator = new();

        public bool IsWorking { get; private set; } = false;

        private void Awake()
        {
            IsWorking = true;
        }

        private void OnEnable()
        {
            _outcomeSimulator.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            _outcomeSimulator.GameOver -= OnGameOver;
        }

        public void ConnectClient(int clientID, Adapter adapter)
        {
            if (_connectedClients.ContainsKey(clientID) || IsWorking == false)
                return;

            _connectedClients.Add(clientID, adapter);
            LoadCombat(clientID);
        }

        public void SimulateCombat(int clientId, IReadOnlyDictionary<Character, CombatMoves> characterMoveDictionary)
        {
            _combatSimulator.SimulateCombat(_connectedClients[clientId], characterMoveDictionary);
        }

        public void SimulateCombatOutcome(int clientId)
        {
            _outcomeSimulator.SimulateCombatOutcome(_connectedClients[clientId], _combatSimulator.LatestCombatCommands);
        }

        public void RestartCombat(int clientId)
        {
            _connectedClients[clientId].ProcessServerCommand(new ReloadCombatCommand());
            _combatSimulator.Reset();
            _outcomeSimulator.Reset();
            LoadCombat(clientId);
        }

        private void LoadCombat(int clientId)
        {
            _outcomeSimulator.CreateSimulationClones(_playerData, _swordsmanData);
            _connectedClients[clientId].ProcessServerCommand(new BuildBattleSceneCommand
                (new Dictionary<Type, CharacterData>()
                {
                    { typeof(Player), _playerData },
                    { typeof(Swordsman), _swordsmanData }
                }));
        }

        private void OnGameOver(Adapter adapter) 
        {
            int clientID = _connectedClients.First(pair => pair.Value == adapter).Key;

            RestartCombat(clientID);
        }
    }
}
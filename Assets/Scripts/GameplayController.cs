using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _winTextBox;
    [SerializeField]
    private Vector3 _playerSpawnPoint = Vector3.one;
    [SerializeField]
    private PlayerController _playerController;
    public void EndGame()
    {
        _winTextBox.text = "You are win!";
        StartCoroutine(_playerController.Move(_playerSpawnPoint));
    }
}

using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
namespace Testes
{

    public class Testes : MonoBehaviour
    {
        [SerializeField] private GameObject     consoleContainer;
        [SerializeField] private TMP_Text       textPrefab;
        [SerializeField] private TMP_InputField inputPrefab;

        public  List<ConsoleLine>          consoleLines = new List<ConsoleLine>();
        private Dictionary<string, string> _inputs      = new Dictionary<string, string>();
        private bool                       _control     = false;
        private bool                       _canPrint    = true;

        private TMP_InputField _consoleInput;
        private ConsoleLine    _consoleData;

        public string nome
        {
            get;
            set;
        }

        private void Start()
        {
            Print("Olá mundo!");
            Print("Este é meu primeiro programa!");
            Print("----------------------------------");
            InputText("nome");
        }


        private void Update()
        {
            RefreshConsole();
        }
        private void RefreshConsole()
        {
            foreach (ConsoleLine line in consoleLines)
            {
                if (_canPrint)
                {
                    _control = false;
                    Debug.Log("CL: " + consoleLines.Count);
                    if (line.type == ConsoleLine.Type.text)
                    {
                        _canPrint = false;
                        TMP_Text consoleLine = Instantiate(textPrefab, consoleContainer.transform);
                        consoleLine.SetText(line.content);
                        consoleLines.Remove(line);
                        _control  = true;
                        _canPrint = true;
                    }

                    if (line.type == ConsoleLine.Type.input)
                    {
                        _canPrint     = false;
                        _consoleInput = Instantiate(inputPrefab, consoleContainer.transform);

                        _consoleInput.onEndEdit.AddListener((string str) =>
                        {
                            Debug.Log("Input: " + str);
                            Debug.Log("Variavel: " + _consoleData.content);
                            _inputs.TryAdd(_consoleData.content, str);
                            Debug.Log("Tamanho:" + _inputs.Count);
                            _consoleInput.onEndEdit.RemoveAllListeners();
                            _control  = true;
                            _canPrint = true;

                        });
                        _consoleData = line;
                        consoleLines.Remove(line);
                    }
                    StartCoroutine(WaitControl());
                }
            }
        }
        private IEnumerator WaitControl()
        {
            yield return new WaitWhile(() => _control != true);
        }

        private void Print(string text)
        {
            consoleLines.Add(new ConsoleLine() { type = ConsoleLine.Type.text, content = text });

        }

        private void InputText(string variable)
        {
            ConsoleLine cl = new ConsoleLine() { type = ConsoleLine.Type.input, content = variable };
            consoleLines.Add(cl);
        }


        private void Cls()
        {
            foreach (Transform child in consoleContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }



    [Serializable]
    public class ConsoleLine
    {
        public enum Type
        {
            text,
            input
        }
        public Type   type;
        public string content;

        public override string ToString()
        {
            return type + " " + content;
        }
    }

}

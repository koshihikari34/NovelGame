using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.InputSystem;

public class NovelGameManager : MonoBehaviour
{
    [System.Serializable]
    public class Choice
    {
        public string text;
        public int nextId;
    }

    [System.Serializable]
    public class DialogueData
    {
        public int id;
        public string character;
        public string text;
        public string background; // 追加
        public List<Choice> choices;
        public string position; // "left", "right", "both", "none"
        public string leftSprite;
        public string rightSprite;
    }

    public Image backgroundImage; // 背景用ImageをInspectorでセット
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public Text dialogueText;
    public Text characterNameText;
    public string jsonFileName = "dialogue.json";
    public Sprite[] characterSprites;

    private List<DialogueData> dialogueList = new List<DialogueData>();
    private int currentIndex = 0;
    private string lastBackgroundName = null;

    private InputAction clickAction;


    void Start()
    {
        LoadDialogueJSON();
        ShowDialogue();

        // Input System: Clickアクションのセットアップ
        var playerInputActions = new InputActionMap("Player");
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.AddBinding("<Touchscreen>/primaryTouch/tap");
        clickAction.performed += ctx => NextDialogue();
        clickAction.Enable();
    }

    // Updateは不要（Input Systemのイベントで処理）
    void OnDestroy()
    {
        if (clickAction != null)
        {
            clickAction.Disable();
            clickAction.Dispose();
        }
    }

    void LoadDialogueJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(jsonFileName));
        if (jsonFile == null)
        {
            Debug.LogError("JSONファイルが見つかりません: " + jsonFileName);
            return;
        }
        dialogueList = new List<DialogueData>(JsonHelper.FromJson<DialogueData>(jsonFile.text));
    }

    // JsonHelperクラス（UnityのJsonUtilityで配列を扱うためのヘルパー）
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

    void ShowDialogue()
    {
        if (currentIndex >= dialogueList.Count) return;
        DialogueData data = dialogueList[currentIndex];
        dialogueText.text = data.text;
        characterNameText.text = data.character;

        // 背景フェード（ゆっくり）
        if (backgroundImage != null && !string.IsNullOrEmpty(data.background))
        {
            Sprite bgSprite = Resources.Load<Sprite>("Backgrounds/" + data.background);
            if (bgSprite != null)
            {
                if (lastBackgroundName != data.background)
                {
                    StartCoroutine(FadeImage(backgroundImage, bgSprite, 0.7f));
                }
                else
                {
                    // 前回と同じ背景なら即時切り替え（フェードなし）
                    backgroundImage.sprite = bgSprite;
                    Color c = backgroundImage.color;
                    c.a = 1f;
                    backgroundImage.color = c;
                }
                lastBackgroundName = data.background;
            }
            else
            {
                Debug.LogWarning($"背景スプライトが見つかりません: {data.background}");
                backgroundImage.gameObject.SetActive(false);
            }
        }

        // キャラ画像表示処理
        if (data.position == "left")
        {
            if (!string.IsNullOrEmpty(data.leftSprite))
            {
                Sprite sprite = GetCharacterSpriteByName(data.leftSprite);
                if (sprite != null)
                {
                    if (leftCharacterImage == null)
                        Debug.LogError("leftCharacterImageがInspectorでセットされていません");
                    StartCoroutine(FadeImage(leftCharacterImage, sprite, 0.15f));
                    leftCharacterImage.gameObject.SetActive(true);
                    Debug.Log($"左画像表示: {data.leftSprite}");
                }
                else
                {
                    Debug.LogWarning($"スプライトが見つかりません: {data.leftSprite}");
                    leftCharacterImage.gameObject.SetActive(false);
                }
            }
            else
            {
                leftCharacterImage.gameObject.SetActive(false);
            }
            rightCharacterImage.gameObject.SetActive(false);
        }
        else if (data.position == "right")
        {
            if (!string.IsNullOrEmpty(data.rightSprite))
            {
                Sprite sprite = GetCharacterSpriteByName(data.rightSprite);
                if (sprite != null)
                {
                    if (rightCharacterImage == null)
                        Debug.LogError("rightCharacterImageがInspectorでセットされていません");
                    StartCoroutine(FadeImage(rightCharacterImage, sprite, 0.15f));
                    rightCharacterImage.gameObject.SetActive(true);
                    Debug.Log($"右画像表示: {data.rightSprite}");
                }
                else
                {
                    Debug.LogWarning($"スプライトが見つかりません: {data.rightSprite}");
                    rightCharacterImage.gameObject.SetActive(false);
                }
            }
            else
            {
                rightCharacterImage.gameObject.SetActive(false);
            }
            leftCharacterImage.gameObject.SetActive(false);
        }
        else if (data.position == "both")
        {
            if (!string.IsNullOrEmpty(data.leftSprite))
            {
                Sprite leftSprite = GetCharacterSpriteByName(data.leftSprite);
                if (leftSprite != null)
                {
                    if (leftCharacterImage == null)
                        Debug.LogError("leftCharacterImageがInspectorでセットされていません");
                    StartCoroutine(FadeImage(leftCharacterImage, leftSprite, 0.15f));
                    leftCharacterImage.gameObject.SetActive(true);
                    Debug.Log($"左画像表示: {data.leftSprite}");
                }
                else
                {
                    Debug.LogWarning($"スプライトが見つかりません: {data.leftSprite}");
                    leftCharacterImage.gameObject.SetActive(false);
                }
            }
            else
            {
                leftCharacterImage.gameObject.SetActive(false);
            }
            if (!string.IsNullOrEmpty(data.rightSprite))
            {
                Sprite rightSprite = GetCharacterSpriteByName(data.rightSprite);
                if (rightSprite != null)
                {
                    if (rightCharacterImage == null)
                        Debug.LogError("rightCharacterImageがInspectorでセットされていません");
                    StartCoroutine(FadeImage(rightCharacterImage, rightSprite, 0.15f));
                    rightCharacterImage.gameObject.SetActive(true);
                    Debug.Log($"右画像表示: {data.rightSprite}");
                }
                else
                {
                    Debug.LogWarning($"スプライトが見つかりません: {data.rightSprite}");
                    rightCharacterImage.gameObject.SetActive(false);
                }
            }
            else
            {
                rightCharacterImage.gameObject.SetActive(false);
            }
        }
        else
        {
            leftCharacterImage.gameObject.SetActive(false);
            rightCharacterImage.gameObject.SetActive(false);
        }
    }


    // Imageのフェードインのみ＋スプライト切り替え
    private System.Collections.IEnumerator FadeImage(Image img, Sprite nextSprite, float duration)
    {
        if (img == null) yield break;
        // 画像を即座に切り替え、アルファ0でセット
        Color c = img.color;
        c.a = 0f;
        img.color = c;
        img.sprite = nextSprite;
        // フェードイン
        float t = 0f;
        while (t < duration)
        {
            c.a = Mathf.Lerp(0f, 1f, t / duration);
            img.color = c;
            t += Time.deltaTime;
            yield return null;
        }
        c.a = 1f;
        img.color = c;
    }

    void NextDialogue()
    {
        currentIndex++;
        if (currentIndex < dialogueList.Count)
        {
            ShowDialogue();
        }
    }

    Sprite GetCharacterSpriteByName(string spriteName)
    {
        foreach (var sprite in characterSprites)
        {
            if (sprite != null && sprite.name == spriteName)
                return sprite;
        }
        return null;
    }
}


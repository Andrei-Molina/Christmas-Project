using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LetterEditorManager : MonoBehaviour
{
    public TMP_InputField letterHeadingEdit;
    public TMP_InputField letterBodyEdit;
    public TMP_InputField letterClosingEdit;
    public TMP_InputField letterNameEdit;
    public TextMeshProUGUI letterHeadingContent;
    public TextMeshProUGUI letterBodyContent;
    public TextMeshProUGUI letterClosingContent;
    public TextMeshProUGUI letterNameContent;
    public Button applyLetterBtn;

    private const string HeadingKey = "LetterHeading";
    private const string BodyKey = "LetterBody";
    private const string ClosingKey = "LetterClosing";
    private const string NameKey = "LetterName";

    private void Awake()
    {
        // Load saved content from PlayerPrefs
        string savedHeading = PlayerPrefs.GetString(HeadingKey, "Default Heading");
        string savedBody = PlayerPrefs.GetString(BodyKey, "Default Body");
        string savedClosing = PlayerPrefs.GetString(ClosingKey, "Default Closing");
        string savedName = PlayerPrefs.GetString(NameKey, "Default Name");

        // Initialize Input Fields
        letterHeadingEdit.text = savedHeading;
        letterBodyEdit.text = savedBody;
        letterClosingEdit.text = savedClosing;
        letterNameEdit.text = savedName;

        // Initialize Content Displays
        letterHeadingContent.text = savedHeading;
        letterBodyContent.text = savedBody;
        letterClosingContent.text = savedClosing;
        letterNameContent.text = savedName;

        // Add listener to the button
        applyLetterBtn.onClick.AddListener(ApplyLetterContent);
    }

    public void ApplyLetterContent()
    {
        // Save Input Field content to PlayerPrefs
        string heading = letterHeadingEdit.text;
        string body = letterBodyEdit.text;
        string closing = letterClosingEdit.text;
        string name = letterNameEdit.text;

        PlayerPrefs.SetString(HeadingKey, heading);
        PlayerPrefs.SetString(BodyKey, body);
        PlayerPrefs.SetString(ClosingKey, closing);
        PlayerPrefs.SetString(NameKey, name);

        // Update the Content Displays
        letterHeadingContent.text = heading;
        letterBodyContent.text = body;
        letterClosingContent.text = closing;
        letterNameContent.text = name;

        // Ensure PlayerPrefs is saved immediately
        PlayerPrefs.Save();
    }
}

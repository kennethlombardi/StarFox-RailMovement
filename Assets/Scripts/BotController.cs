using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Syrus.Plugins.DFV2Client;
using UnityEngine.UI;
using Events;
using Variables;

public class BotController : MonoBehaviour
{
    public InputField session;

    public Text content;

    public Text chatbotText;

    private DialogFlowV2Client client;

    public GameEvent onFoxForceFour;

    public GameEvent onHorizontalSplit;

    public GameEvent groupUp;

    public Text finalResultText;

    // Start is called before the first frame update
    void Start()
    {
        // client = GetComponent<DialogFlowV2Client>();

        // client.ChatbotResponded += LogResponseText;
        // client.DetectIntentError += LogError;
        // client.ReactToContext("DefaultWelcomeIntent-followup",
        //     context => Debug.Log("Reacting to welcome followup"));
        // client.SessionCleared += sess => Debug.Log("Cleared session " + session);
        // client.AddInputContext(new DF2Context("userdata", 1, ("name", "George")), name);



        // Dictionary<string, object> parameters = new Dictionary<string, object>()
        // {
        //     { "name", "George" }
        // };
        // client.DetectIntentFromEvent("test-inputcontexts", parameters, name);


        client = GetComponent<DialogFlowV2Client>();

        client.ChatbotResponded += LogResponseText;
        client.DetectIntentError += LogError;

        // Send additional parameters to event
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "name", "George" }
        };
        // client.DetectIntentFromEvent("event-name", parameters, session.text);
        // client.DetectIntentFromText("fox force four", "one");
        // SendText("fox force four");
    }

    private void LogResponseText(DF2Response response)
    {
        Debug.Log(JsonConvert.SerializeObject(response, Formatting.Indented));
        Debug.Log(name + " said: \"" + response.queryResult.fulfillmentText + "\"");
        chatbotText.text = response.queryResult.fulfillmentText;
        if (response.queryResult.intent["displayName"].Equals("fox-force-four"))
        {
            onFoxForceFour.Raise();
        }

        if (response.queryResult.intent["displayName"].Equals("horizontal-split"))
        {
            onHorizontalSplit.Raise();
        }

        if (response.queryResult.intent["displayName"].Equals("group-up"))
        {
            groupUp.Raise();
        }
    }

    private void LogError(DF2ErrorResponse errorResponse)
    {
        Debug.LogError(string.Format("Error {0}: {1}", errorResponse.error.code.ToString(),
            errorResponse.error.message));
    }


    public void SendText(string toSend)
    {
        // DF2Entity name0 = new DF2Entity("George", "George");
        // DF2Entity name1 = new DF2Entity("Greg", "Greg");
        // DF2Entity potion = new DF2Entity("Potion", "Potion", "Cure", "Healing potion");
        // DF2Entity antidote = new DF2Entity("Antidote", "Antidote", "Poison cure");
        // DF2EntityType names = new DF2EntityType("names", DF2EntityType.DF2EntityOverrideMode.ENTITY_OVERRIDE_MODE_SUPPLEMENT,
        //     new DF2Entity[] { name0, name1 });
        // DF2EntityType items = new DF2EntityType("items", DF2EntityType.DF2EntityOverrideMode.ENTITY_OVERRIDE_MODE_SUPPLEMENT,
        //     new DF2Entity[] { potion, antidote });
        // client.AddEntityType(names, name);
        // client.AddEntityType(items, name);

        client.DetectIntentFromText(toSend, "one");
    }

    public void SendFinalResult()
    {
        SendText(finalResultText.text);
    }


    public void SendEvent()
    {
        client.DetectIntentFromEvent(content.text,
            new Dictionary<string, object>(), session.text);
    }

    public void Clear()
    {
        client.ClearSession(name);
    }
}
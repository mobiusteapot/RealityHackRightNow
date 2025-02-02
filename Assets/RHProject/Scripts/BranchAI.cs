using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using UnityEngine;

public class  BranchAi : MonoBehaviour

{
    private readonly List<Message> _msgs = new();
    private Dictionary<string, List<string>> _tree;

    private void Awake()
    {
        _tree = new Dictionary<string, List<string>>();
    }
    
    public async void Summarize(string userInput)
    {
        Debug.Log("Summarizing...");

        // Add system instruction to summarize the input and extract a keyword
        _msgs.Add(new Message(
            Role.System,
            "You are a summary assistant that help enhance human communication"+
            "Summarize each user input with one short sentence."+
            "The input is transcribed from live conversation"+
            "Also, provide a single keyword that best represents the text, separate it by saying, Keyword:"
        ));
        
        _msgs.Add(new Message(Role.User, userInput));

        // Initialize OpenAI client
        var api = new OpenAIClient(KeysStorage.Data.openai_key);

        // Create a chat request
        var chatRequest = new ChatRequest(_msgs, null, "auto", Model.GPT4o);
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        // Get the model's response
        var choice = response.FirstChoice;
        _msgs.Add(choice.Message);

        // Split response into summary and keyword
        string responseText = choice.Message.Content.ToString();
        string[] parts = responseText.Split(new[] { "Keyword:" }, StringSplitOptions.RemoveEmptyEntries);

        string summary = parts.Length > 0 ? parts[0].Trim() : "No summary provided.";
        string keyword = parts.Length > 1 ? parts[1].Trim() : "No keyword provided.";
        Debug.Log("Summary: " + summary);
        Debug.Log(keyword);
    }


    public async void StartConversation(string initialUserInput)
    {
        Debug.Log("Starting conversation...");

        _msgs.Add(new Message(
            Role.System,
            "You are a conversation assistant, your goal is to make human conversation human. " +
            "Begin by creating an initial tree structure based on the user's input conversation. " +
            "Include main points, distinct ideas and general categories to organize ideas."
        ));

        _msgs.Add(new Message(Role.User, initialUserInput));
        var api = new OpenAIClient(KeysStorage.Data.openai_key);

        var chatRequest = new ChatRequest(_msgs, null, "auto", Model.GPT4o);
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        var choice = response.FirstChoice;
        
        _msgs.Add(choice.Message);

        ParseInitialTree(choice.Message.Content.ToString());
        
        //PrintTree();
    }

    private void ParseInitialTree(string gptResponse)
    {
        Debug.Log("Parsing initial tree");

        var lines = gptResponse.Split('\n');
        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                var category = line.Trim('-').Trim();
                if (!_tree.ContainsKey(category))
                {
                    _tree[category] = new List<string>();
                }
            }
        }
    }

    public async void ProcessText(string inputText)
    {
        Debug.Log("Processing text");

        _msgs.Add(new Message(Role.User, inputText));
        
        var api = new OpenAIClient(KeysStorage.Data.openai_key);

        var chatRequest = new ChatRequest(
            _msgs,
            null,
            "auto",
            Model.GPT4o
        );
        
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
        var choice = response.FirstChoice;
        _msgs.Add(choice.Message);

        var summary = choice.Message.Content.ToString();
        string relevantBranch = await DetermineRelevantBranch(summary);
        
        AddToTree(relevantBranch, summary);
        
        //PrintTree();
    }

    private async Task<string> DetermineRelevantBranch(string summary)
    {
        _msgs.Add(new Message(
            Role.System,
            "Based on the following tree structure, determine the most relevant branch for the given summary. " +
            "If no branch is relevant, suggest adding it under the root.\n\n" +
            "Tree Structure:\n" + GetTreeStructure() +
            "\n\nSummary: " + summary
        ));
        
        var api = new OpenAIClient(KeysStorage.Data.openai_key);

        var chatRequest = new ChatRequest(_msgs, null, "auto", Model.GPT4o);
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
    
        return response.FirstChoice.Message.Content.ToString();
    }

    private string GetTreeStructure()
    {
        var structure = "";
        foreach (var branch in _tree)
        {
            structure += "- " + branch.Key + "\n";
            foreach (var subItem in branch.Value)
            {
                structure += "  - " + subItem + "\n";
            }
        }
        return structure;
    }

    private void AddToTree(string branch, string summary)
    {
        if (!_tree.ContainsKey(branch))
        {
            _tree[branch] = new List<string>();
        }
        _tree[branch].Add(summary);
    }

    private void PrintTree()
    {
        Debug.Log("Tree Structure:");
        foreach (var branch in _tree)
        {
            Debug.Log($"- {branch.Key}");
            foreach (var subItem in branch.Value)
            {
                Debug.Log($"  - {subItem}");
            }
        }
    }
}




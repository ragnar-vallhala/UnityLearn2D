using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messages
{
    
    List<string> _messages;
    List<float> _timeOfMeassages;
    [SerializeField] float _timeOfDisplay = 1;
    [SerializeField] float _opacity;
    [SerializeField] Color _color;

    [SerializeField] int _maxMessages = 5;

    public Messages(float timeOfDisplay, int maxMessages){
        _messages = new();
        _timeOfMeassages = new();
        _timeOfDisplay = timeOfDisplay;
        _maxMessages = maxMessages;
        return;
    }

    public void Update(){
        
        if(_timeOfMeassages.Count>0 && Time.time - _timeOfMeassages[0]>_timeOfDisplay){
        _messages.Remove(_messages[0]);
        _timeOfMeassages.Remove(_timeOfMeassages[0]);
        }
        
    }
    public void AddMessage(string text){
        if(_maxMessages<_messages.Count){
            _messages.Remove(_messages[0]);
            _timeOfMeassages.Remove(_timeOfMeassages[0]);
        }

        _messages.Add(text);
        _timeOfMeassages.Add(Time.time);
    }

    public List<string> GetMeassages(){
        return _messages;
    }


}

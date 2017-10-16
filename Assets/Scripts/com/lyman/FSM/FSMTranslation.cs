using System;
using System.Collections.Generic;
using System.Text;

public class FSMTranslation
{
    private string name;
    private FSMState input;
    private FSMState output;    
    private FSMAction action;

    public string Name { get { return name; } }
    public FSMAction Action { get { return action; } }
    public FSMState Input { get { return input; } }
    public FSMState Output { get { return output; } }    

    public FSMTranslation(FSMState input, FSMState output):this("NoneName",input,output,null)
    {
    }
    public FSMTranslation(string name, FSMState input, FSMState output, FSMAction action)
    {
        this.name = name;
        this.input = input;
        this.output = output;        
        this.action = action;
    }

    public FSMTranslation(FSMState input, FSMState output, FSMAction action)
        : this("NoneName", input, output, action)
    {
    }    
}

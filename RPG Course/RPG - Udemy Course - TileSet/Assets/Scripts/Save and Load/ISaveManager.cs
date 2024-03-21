using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Interfaces can be implemented on different scripts, same function but can do different functionality.
//When put on a class, interface MUST be implemented.
// When we did on hover for UI (pointerEnter and pointerExit), we added those functions by implementing their interface.
//Just templates, the classes implementing it supply the function.
public interface ISaveManager
{
    void LoadData(GameData _data);

    void SaveData(ref GameData _data);    //Reference sends the actual ref to the original variable, not a copy like normal arguments. So changing the parameter it receives also changes the original.



}

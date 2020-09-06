using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITool{
    void Interact();
    void GetNewPos(Vector3 pos);
    void GetCurrPos(Vector3 pos);
    void BeingInteractedWith(bool state);
}

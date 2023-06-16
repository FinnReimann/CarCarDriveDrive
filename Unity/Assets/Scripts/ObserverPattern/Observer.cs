using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{

    // Receive update from subject
    void CCDDUpdate(CCDDEvents e);
}

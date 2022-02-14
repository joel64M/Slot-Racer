using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace NameSpaceName {

    public class VirtualCameraScript : MonoBehaviour
    {

        #region Variables
        CinemachineVirtualCamera cvm;
    #endregion

    #region Builtin Methods

    #endregion

    #region Custom Methods
        public void SetCameraTarget(Transform t)
        {
            cvm = GetComponent<CinemachineVirtualCamera>();

            cvm.Follow = t;
            cvm.LookAt = t;
        }
    #endregion

    }
}

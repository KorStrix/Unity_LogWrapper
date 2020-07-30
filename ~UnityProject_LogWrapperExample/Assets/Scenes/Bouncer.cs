#region Header
/*	============================================
 *	Author 			    	: #AUTHOR#
 *	Initial Creation Date 	: #CREATIONDATE#
 *	Summary 		        : 
 *  Template 		        : New Behaviour For Unity Editor V2
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bouncer : MonoBehaviour
{
	/* const & readonly declaration             */

	/* enum & struct declaration                */

	/* public - Field declaration               */

    [Header("튈때 높이")]
    public float fHeight = 10f;
    [Header("튈때 x폭")]
    public float fRandom_XGap = 10f;

    [Header("중력 추가 가속도 곱")]
    public float fGravity_Multiply = 10f;

    /* protected & private - Field declaration  */

    Rigidbody _pRigidbody = null;

    // ========================================================================== //

    /* public - [Do~Something] Function 	        */


    // ========================================================================== //

    /* protected - [Override & Unity API]       */

    private void Awake()
    {
        _pRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _pRigidbody.AddForce(new Vector3(Random.Range(-fRandom_XGap, fRandom_XGap), fHeight, Random.Range(-fRandom_XGap, fRandom_XGap)));
    }

    private void Update()
    {
        Vector3 vecGravity = Physics.gravity;
        vecGravity.y *= fGravity_Multiply;

        _pRigidbody.AddForce(vecGravity);
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    #endregion Private
}

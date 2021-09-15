using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFromListOnDestroy : MonoBehaviour {

	private IList<GameObject> ownerList;

	public void setOwnerList(ref IList<GameObject> ownerList)
	{
		this.ownerList = ownerList;
	}

	private void OnDestroy()
	{
		if (ownerList != null)
		{
			ownerList.Remove(gameObject);
		}
	}



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialModify : MonoBehaviour {
	public int offsetFactor, offsetUnit;
    public int queue = -1;

	private Material material;

    bool CheckMaterial()
    {
        if (this.material != null)
            return false;

        var renderer = this.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("[MaterialModify]not find renderer, gameObject = " + this.gameObject.name);
            this.enabled = false;
            return false;
        }

        this.material = renderer.sharedMaterial;
        if (this.material == null)
        {
            Debug.LogWarning("[MaterialModfiy]not find sharedMaterial, gameObject = " + this.gameObject.name);
            this.enabled = false;
            return false;
        }

        return true;
    }

    void Awake () {
        if (!this.CheckMaterial())
            return;

        this.SetOffset();
        this.SetQueue();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!this.CheckMaterial())
            return;

        this.SetOffset();
        this.SetQueue();
    }
#endif

    void SetQueue()
    {
        if (this.queue == -1)
            return;
        this.material.renderQueue = this.queue;
    }

    void SetOffset()
    {
        this.material.SetInt("_OffsetFactor", this.offsetFactor);
        this.material.SetInt("_OffsetUnits", this.offsetUnit);
    }
}

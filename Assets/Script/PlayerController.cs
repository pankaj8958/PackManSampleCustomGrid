using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnPlayerInteract(collision.gameObject.tag);
    }

    private void OnPlayerInteract(string tagName)
    {
        string typeTag = tagName.Contains("_") ? tagName.Split('_')[0] : tagName;
        switch(typeTag)
        {
            case "Wall":
                OnHitOnBoundary(tagName);
                break;
            case "":
                break;
        }
    }

    private void OnHitOnBoundary(string tagName)
    {
        Transform resPosObj = null;
        switch(tagName)
        {
            case "Wall_Up":
                resPosObj = GridAlignManager.Instance.GetBoundaryTransform(Wall.WallDown);
                if (resPosObj != null)
                {
                    transform.position = new Vector2(transform.position.x, resPosObj.position.y + 0.1f);
                }
                break;
            case "Wall_Down":
                resPosObj = GridAlignManager.Instance.GetBoundaryTransform(Wall.WallUp);
                if (resPosObj != null)
                {
                    transform.position = new Vector2(transform.position.x, resPosObj.position.y - 0.1f);
                }
                break;
            case "Wall_Left":
                resPosObj = GridAlignManager.Instance.GetBoundaryTransform(Wall.WallRight);
                if (resPosObj != null)
                {
                    transform.position = new Vector2(resPosObj.position.x - 0.1f, transform.position.y);
                }
                break;
            case "Wall_Right":
                resPosObj = GridAlignManager.Instance.GetBoundaryTransform(Wall.WallLeft);
                if (resPosObj != null)
                {
                    transform.position = new Vector2(resPosObj.position.x + 0.1f, transform.position.y);
                }
                break;
        }
    }

    public void OnDirectionChange(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

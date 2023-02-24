using DG.Tweening;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField] private PlayerMovement move;
    //[SerializeField]private AnimationScript anim;
    private SpriteRenderer sr;
    public Transform ghostsParent;
    public Color trailColor;
    public Color fadeColor;
    public float ghostInterval;
    public float fadeTime;

    public float fadeDistance;

    private void Start()
    {


        sr = GetComponent<SpriteRenderer>();
    }

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++)
        {

            Transform currentGhost = ghostsParent.GetChild(i);
            float positionx = move.transform.position.x;
            positionx = (positionx > 0) ? -fadeDistance : fadeDistance;
            Vector3 pos = new Vector3(move.transform.position.x + positionx, move.transform.position.y, move.transform.position.z);
            s.AppendCallback(() => currentGhost.position = pos);
            // s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = anim.sr.flipX);
            // s.AppendCallback(()=>currentGhost.GetComponent<SpriteRenderer>().sprite = anim.sr.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(ghostInterval);
        }
    }

    public void FadeSprite(Transform current)
    {
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

}

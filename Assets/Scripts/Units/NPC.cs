using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class NPC : MonoBehaviour
{
    public GameObject Dialog;
    public Vector3 RetreatCoords;

    GameObject target;
    bool canCollide = true;

    // Start is called before the first frame update
    void Start()
    {
        Dialog?.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canCollide && collision.collider.gameObject.tag == "Player")
        {
            //Debug.Log("Retreat coords of " + gameObject.name + " is " + RetreatCoords);
            target = collision.collider.gameObject;
            canCollide = false;

            var panel = Dialog.GetComponent<Image>();
            panel.DOFade(0, 0);
            panel.DOFade(0.8f, 0.5f);

            var image = Dialog.transform.GetChild(0).GetComponent<Image>();
            var p = image.GetComponent<RectTransform>();
            image.DOFade(0, 0);
            p.anchoredPosition = new Vector2(p.anchoredPosition.x + 50, p.anchoredPosition.y);

            Dialog.SetActive(true);
            image.DOFade(1, 0.5f);
            p.DOAnchorPosX(p.anchoredPosition.x - 50, 0.5f);

            collision.collider.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            collision.collider.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            collision.collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            transform.Find("/Managers").GetComponent<LandMovementManager>().Stop();
        }

    }
    public void Attack()
    {
        ClosePanel();
    }

    public void Retreat()
    {
        ClosePanel();
    }

    private void ClosePanel()
    {
        var panel = Dialog.GetComponent<Image>();
        panel.DOFade(0, 0.5f);

        var image = Dialog.transform.GetChild(0).GetComponent<Image>();
        image.DOFade(0, 0.5f);

        StartCoroutine(ClosePanelNow());
        target.GetComponent<NavMeshAgent>().isStopped = false;
        target.GetComponent<NavMeshAgent>().SetDestination(RetreatCoords);
    }

    IEnumerator ClosePanelNow()
    {
        yield return new WaitForSeconds(0.5f);

        Dialog.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        canCollide = true;
    }

    public void Other(int index)
    {

    }
}

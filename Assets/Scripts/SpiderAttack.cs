using UnityEngine;

public class SpiderAttack : MonoBehaviour
{

    private float speed = .5f;

    private Transform target;

    private Spider_Manager spiderScript;

    public GameObject hit;
    private ParticleSystem hitParticle;

    private void Awake()
    {

        hit = Instantiate(hit);
        hitParticle = hit.GetComponent<ParticleSystem>();

        hitParticle.Stop();

    }

    private void Start()
    {

        hit.transform.parent = transform.parent;
        hit.transform.localScale = transform.localScale;

    }

    private void Update()
    {

        if (target)
        {

            transform.LookAt(target);
            transform.position += transform.forward * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) <= 0.1f || !target.gameObject.activeSelf)
            {

                spiderScript.CalculateDamage(tag);

                hit.transform.position = transform.position;
                hitParticle.Play();

                transform.position += new Vector3(0, -10, 10);

                gameObject.SetActive(false);

            }

        }

    }

    public void SetTarget(Transform enemy)
    {

        target = enemy;
        spiderScript = enemy.GetComponent<Spider_Manager>();

    }

}
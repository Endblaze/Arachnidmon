using System.Collections;
using UnityEngine;

public class Spider_Manager : MonoBehaviour
{

    private int dir;

    private float life = 100;

    private float dmg = 20;

    private bool attack, showDeath = true;

    private Transform target;

    public enum SpiderTypes { Grass, Fire, Water }
    public SpiderTypes type;

    public GameObject spiderAttack;
    private SpiderAttack attackScript;

    public TextMesh lifeText, counterText;

    private float respawnTime = 10;

    private Animator anim;

    private void Start()
    {

        counterText = Instantiate(counterText.gameObject).GetComponent<TextMesh>();
        counterText.transform.parent = transform.parent;
        counterText.gameObject.SetActive(false);

        spiderAttack = Instantiate(spiderAttack);
        spiderAttack.transform.parent = transform.parent;
        attackScript = spiderAttack.GetComponent<SpiderAttack>();

        spiderAttack.transform.parent = transform.parent;

        spiderAttack.transform.localScale /= 6;

        spiderAttack.tag = type.ToString();
        spiderAttack.SetActive(false);

        anim = GetComponent<Animator>();

        StartCoroutine("AIMove");

    }

    private void Update()
    {

        if (life <= 0)
        {

            Death();
            return;

        }

        if (attack)
        {

            Attack();

        }
        else
        {
            transform.Rotate(0, dir * 100 * Time.deltaTime, 0);
        }

    }

    private void LateUpdate()
    {

        if (life <= 0 && showDeath)
        {

            if (showDeath)
            {
                anim.SetTrigger("Dead");
                showDeath = false;
            }

            return;

        }

        if (!attack)
        {
            anim.SetInteger("Dir", dir);
        }
        else
        {
            anim.SetInteger("Dir", 0);
        }

    }

    private void GetDamage(float dmg)
    {

        life -= dmg;

        if(life < 0) {

            life = 0;

        }

        lifeText.text = (int)life + " HP";

        if (life <= 20 && lifeText.color != Color.red)
        {
            lifeText.color = Color.red;
            return;
        }

        if (life <= 50 && lifeText.color != Color.yellow)
        {
            lifeText.color = Color.yellow;
            return;
        }

        lifeText.color = Color.green;

    }

    private void Attack()
    {

        if (!spiderAttack.activeSelf)
        {

            Vector3 targetPos = target.position;

            targetPos.y = transform.position.y;

            transform.LookAt(targetPos);

            spiderAttack.transform.position = transform.position;
            attackScript.SetTarget(target);

            anim.SetTrigger("Attack");

            spiderAttack.SetActive(true);

            if (!target.gameObject.activeSelf)
            {

                target.gameObject.SetActive(false);
                spiderAttack.SetActive(false);
                target = null;
                attack = false;

            }

        }

    }

    private void Death()
    {

        transform.localScale -= Vector3.one * Time.deltaTime;

        if(transform.localScale.x <= 0.01f)
        {

            counterText.gameObject.transform.position = transform.position;
            counterText.text = (int)respawnTime + "";

            counterText.gameObject.SetActive(true);

            Invoke("ResetSpider", respawnTime);

            gameObject.SetActive(false);

        }

    }

    IEnumerator AIMove()
    {

        while (true)
        {

            dir = Random.Range(-1, 2);

            yield return new WaitForSeconds(Random.Range(.1f,1));

            dir = 0;

            yield return new WaitForSeconds(Random.Range(.1f, 1));

        }

    }

    public void CalculateDamage(string tag)
    {

            SpiderTypes attackType = (SpiderTypes)System.Enum.Parse(typeof(SpiderTypes), tag);

            if (type == SpiderTypes.Grass)
            {
                switch (attackType)
                {
                    case SpiderTypes.Grass:
                        GetDamage(dmg);
                        break;
                    case SpiderTypes.Fire:
                        GetDamage(dmg * 2);
                        break;
                    case SpiderTypes.Water:
                        GetDamage(dmg / 2);
                        break;
                }
            }

            if (type == SpiderTypes.Fire)
            {
                switch (attackType)
                {
                    case SpiderTypes.Grass:
                        GetDamage(dmg / 2);
                        break;
                    case SpiderTypes.Fire:
                        GetDamage(dmg);
                        break;
                    case SpiderTypes.Water:
                        GetDamage(dmg * 2);
                        break;
                }
            }

            if (type == SpiderTypes.Water)
            {
                switch (attackType)
                {
                    case SpiderTypes.Grass:
                        GetDamage(dmg * 2);
                        break;
                    case SpiderTypes.Fire:
                        GetDamage(dmg / 2);
                        break;
                    case SpiderTypes.Water:
                        GetDamage(dmg);
                        break;
                }
            }

    }

    private void ResetSpider()
    {

        GetDamage(-100);

        gameObject.SetActive(true);

        transform.localScale = Vector3.one;

        counterText.gameObject.SetActive(false);

    }

    private void OnTriggerStay(Collider c)
    {

        if (target == null && c.tag == "Spider")
        {
            target = c.transform;
            attack = true;
        }

    }

    private void OnTriggerExit(Collider c)
    {

        if(c.tag == "Spider")
        {

            target = null;
            attack = false;

            if (spiderAttack.activeSelf)
            {
                spiderAttack.SetActive(false);
            }

        }
        
    }

}
using UnityEngine;

public class ObjectFalling : MonoBehaviour
{
    public float _weight;

    public AudioSource _sound;
    public EnemyAI _enemy;

    float _fallTime;
    bool _falling = false;

    private void Update()
    {
        if (_falling) _fallTime += Time.deltaTime * _weight;
    }

    public void OnCollisionExit(Collision collision)
    {
        _fallTime = 0;
        _falling = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        _falling = false;

        if (Vector3.Distance(this.transform.position, _enemy.gameObject.transform.position) < _fallTime)
        {
            _enemy.HearSound(this.transform.position);
        }
    }
}

using UnityEngine.SceneManagement;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    Rigidbody _rb;
    AudioSource _as;

    [SerializeField] float rotationSpeed = 70f;
    [SerializeField] float flySpeed = 50f;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem flyParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem finishParticle;

    bool collisionStatus = false;

    enum State
    {
        Playing,
        Dead,
        LoadNextLevel
    };

    State state;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _as = GetComponent<AudioSource>();
        state = State.Playing;
    }

    void Update()
    {
        if (state == State.Playing)
        {
            RocketLaunch();
            RocketRotation();
            DebugKeys();
        }
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionStatus = !collisionStatus;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Playing || collisionStatus)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "FriendlyObject":
                break;
            case "Finish":
                Finish();
                break;
            case "Fuel":
                break;
            default:
                Death();
                break;
        }
    }

    void Death()
    {
        state = State.Dead;
        _as.Stop();
        deathParticle.Play();
        _as.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", 2.5f);
    }

    void Finish()
    {
        state = State.LoadNextLevel;
        _as.Stop();
        finishParticle.Play();
        _as.PlayOneShot(finishSound);
        Invoke("LoadNextLevel", 2.5f);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    void RocketLaunch()
    {
        float flySpeedDelta = flySpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) 
        {
            _rb.AddRelativeForce(Vector3.up * flySpeedDelta);
            if (!_as.isPlaying)
                _as.PlayOneShot(flySound);
            flyParticle.Play();
        }
        else
        {
            _as.Pause();
            flyParticle.Stop();
        }
    }

    void RocketRotation()
    {
        float rotationSpeedDelta = rotationSpeed * Time.deltaTime; 
        _rb.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeedDelta);
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            transform.Rotate(Vector3.back * rotationSpeedDelta);
        }
        _rb.freezeRotation = false;
    }
}

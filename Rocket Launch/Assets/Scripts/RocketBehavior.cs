using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    Rigidbody _rb;
    AudioSource _as;

    [SerializeField] Text textFuel;
    [SerializeField] int fuelTotal = 2000;
    [SerializeField] int fuelApply = 5;
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
        if (state == State.Playing || fuelTotal < 5)
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
                GetFuel(500, collision.gameObject);
                break;
            default:
                Death();
                break;
        }
    }
    void GetFuel(int fuelAdd, GameObject fuelObj)
    {
        fuelObj.GetComponent<BoxCollider>().enabled = false;
        fuelTotal += fuelAdd;
        Destroy (fuelObj);
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
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }

    void RocketLaunch()
    {
        float flySpeedDelta = flySpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && fuelTotal > 0) 
        {
            fuelTotal -= Mathf.RoundToInt(fuelApply*Time.deltaTime);
            textFuel.text = fuelTotal.ToString();
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

using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockPlacer : MonoBehaviour, IPausible
{
    public bool Paused { get; set; }

    private const KeyCode k_PlacementButton = KeyCode.Space;
    private const string k_BlockTag = "Block";
    private const string k_CameraLimitLayer = "CameraLimits";
    private const string k_ScoreTextTag = "ScoreText";
    private const string k_GameOverScene = "GameOver";

    private Text m_ScoreText;

    private Rigidbody2D m_HeldBlock = null;
    private bool m_WaitingOnNextBlock = true;
    private float m_HorizontalDropPosition;
    
    private bool m_ReadyToPlay = false;
    private float m_HeightOfBlockInPixels;
    private float m_WidthOfBlockInPixels;

    private Coroutine m_PlacerMover;
    
    [SerializeField]
    private Transform m_RightBorderHorizontalPosition;
    [SerializeField]
    private Transform m_LeftBorderHorizontalPosition;
    [SerializeField]
    private float m_TimeBetweenBlocks;
    [SerializeField]
    private float m_ScreenSizeGrowthRate;
    [SerializeField]
    private float m_MaxScreenSizeGrowth;
    [SerializeField]
    private AnimationCurve m_SideToSideTimePerLandedBlocks;

    // Use this for initialization
    void Start()
    {
        m_ScoreText = GameObject.FindGameObjectWithTag(k_ScoreTextTag).GetComponent<Text>();

        m_HeldBlock = ObjectPoolManager.PullObject(k_BlockTag).GetComponent<Rigidbody2D>();
        m_HeldBlock.GetComponent<Collider2D>().enabled = false;
        m_HeldBlock.gravityScale = 0;

        SpriteRenderer sprite = m_HeldBlock.GetComponent<SpriteRenderer>();
        //Camera.main.orthographicSize = Screen.height / 2 / sprite.sprite.pixelsPerUnit;
        m_HeightOfBlockInPixels = sprite.sprite.pixelsPerUnit;
        m_WidthOfBlockInPixels = sprite.sprite.texture.width / (sprite.sprite.texture.height / m_HeightOfBlockInPixels);
        StartCoroutine(scrollUpForGameStart());
        
        m_HorizontalDropPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - m_WidthOfBlockInPixels, Screen.height / 2, transform.position.z)).x;
       
        m_PlacerMover = StartCoroutine(movePlacerPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ReadyToPlay)
        {
            if (m_HeldBlock)
            {
                m_HeldBlock.transform.position = new Vector2(m_HorizontalDropPosition, transform.position.y);

#if UNITY_EDITOR || UNITY_STANDALONE
                if (Input.GetKeyDown(k_PlacementButton))
                {
                    dropBlock();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    EventSystem eventSystem = EventSystem.current;
                    if(!eventSystem.IsPointerOverGameObject() && eventSystem.currentSelectedGameObject == null)
                    {
                        dropBlock();
                    }
                }
#elif UNITY_ANDROID || UNITY_IOS || UNITY_BLACKBERRY || UNITY_WP8 || UNITY_TIZEN
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                EventSystem eventSystem = EventSystem.current;
                if(!eventSystem.IsPointerOverGameObject() && eventSystem.currentSelectedGameObject == null)
                {
                    dropBlock();
                }
            }
#endif
            }
            else
            {
                if (m_WaitingOnNextBlock)
                {
                    StartCoroutine(readyNextBlock());
                }
            }
        }

        m_ScoreText.text = Game.Score.ToString();
    }

    private void dropBlock()
    {
        if (!Paused)
        {
            Game.AddPoint();
            if(Game.IsNewHighScore())
            {
                StartCoroutine(HighScoreMessage());
            }

            m_HeldBlock.gravityScale = 1;
            m_HeldBlock = null;
            m_WaitingOnNextBlock = true;

            StopCoroutine(m_PlacerMover);

            StartCoroutine(scrollUp1Line());
        }
    }

    private IEnumerator HighScoreMessage()
    {
        Transform recordText = GameObject.Find("Record").transform;
        recordText.GetComponent<Text>().enabled = true;

        float rotationSpeed = 3;

        while(rotationSpeed > 1)
        {
            rotationSpeed -= Time.deltaTime;
            recordText.Rotate(0, 0, rotationSpeed * 1.5f);
            yield return null;
        }
        
        recordText.GetComponent<Text>().enabled = false;
        recordText.rotation = Quaternion.identity;
    }

    private IEnumerator movePlacerPosition()
    {
        while (!m_ReadyToPlay)
        {
            yield return null;
        }

        bool movingLeft = true;
        
        float timeAtStart = Time.time;
        float timeAtFinish = timeAtStart + m_SideToSideTimePerLandedBlocks.Evaluate(Game.Score);
         
        while (true)
        {
            float delayTime = Time.time;
            while (Paused)
            {
                yield return null;
            }
            delayTime = Time.time - delayTime;
            timeAtFinish += delayTime;
            timeAtStart += delayTime;

            float percentComplete = (Time.time - timeAtStart) / (timeAtFinish - timeAtStart);
            
            float offset = 6;
            m_HorizontalDropPosition = Mathf.Lerp(movingLeft ? m_RightBorderHorizontalPosition.position.x - offset : m_LeftBorderHorizontalPosition.position.x + offset, movingLeft ? m_LeftBorderHorizontalPosition.position.x + offset : m_RightBorderHorizontalPosition.position.x - offset, percentComplete);
            
            if (percentComplete >= 1)
            {
                timeAtStart = Time.time;
                timeAtFinish = timeAtStart + m_SideToSideTimePerLandedBlocks.Evaluate(Game.Score);
                movingLeft = !movingLeft;
            }

            yield return null;
        }
    }

    private IEnumerator readyNextBlock()
    {
        m_WaitingOnNextBlock = false;

        yield return new WaitForSeconds(m_TimeBetweenBlocks);
        while (Paused)
        {
            yield return null;
        }

        if(!Game.Lost)
        {
            m_HeldBlock = ObjectPoolManager.PullObject(k_BlockTag).GetComponent<Rigidbody2D>();
            m_HeldBlock.gravityScale = 0;
            m_PlacerMover = StartCoroutine(movePlacerPosition());
        }
    }

    private IEnumerator scrollUpForGameStart()
    {
        for (int i = 0; i < Mathf.RoundToInt(Camera.main.orthographicSize) / 2 - 3; ++i)
        {
            yield return StartCoroutine(scrollUp1Line(0.5f));

            yield return null;
        }

        m_ReadyToPlay = true;
        m_HeldBlock.GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator scrollUp1Line(float i_TimePerScroll = 1)
    {
        float timeAtStart = Time.time;
        float timeAtFinish = timeAtStart + i_TimePerScroll;
        float percentComplete = 0;

        float startingVerticalPosition = transform.position.y;
        //Vector3 currentScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(currentScreenPosition + Vector3.up * (m_BlockTexturePixelHeight / m_HeightOfBlockInPixels + 2));
        //float endingVerticalPosition = targetWorldPosition.y;
        float endingVerticalPosition = startingVerticalPosition + 1.1f;

        do
        {
            float delayTime = Time.time;
            while (Paused)
            {
                yield return null;
            }
            delayTime = Time.time - delayTime;
            timeAtFinish += delayTime;
            timeAtStart += delayTime;

            percentComplete = (Time.time - timeAtStart) / (timeAtFinish - timeAtStart);

            float newYPosition = Mathf.Lerp(startingVerticalPosition, endingVerticalPosition, percentComplete);
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

            Camera.main.orthographicSize = Math.Min(Camera.main.orthographicSize + Time.deltaTime * m_ScreenSizeGrowthRate, m_MaxScreenSizeGrowth);

            yield return null;
        } while (percentComplete < 1);
    }

    public IEnumerator LostGame()
    {
        if(!Game.Lost)
        {
            Game.Lost = true;
                        
            m_WaitingOnNextBlock = false;
            if(m_HeldBlock)
            {
                m_HeldBlock.gameObject.SetActive(false);
            }
            m_HeldBlock = null;

            while(Vector3.Distance(transform.position, new Vector3(0, 0, transform.position.z)) > 1)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(3);

            Application.LoadLevel(k_GameOverScene);
        }
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Resume()
    {
        Paused = false;
    }
}

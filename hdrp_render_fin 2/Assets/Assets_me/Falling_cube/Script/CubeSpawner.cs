using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject cubePrefab2;
    public int cubesPerFrameMin = 1;
    public int cubesPerFrameMax = 8;
    public Vector3 spawnArea = new Vector3(11, 11, 11);
    public Transform groundPlane;
    public float duration = 300f; // 5분

    private float elapsed = 0f;
    private float elapsedTime = 0f;
    private bool spawning = false;

    private float duration1 = 5.0f; // 변환 시간
    private Vector3 spaceBarPosition;
    private Vector3 targetSize = new Vector3(0.001f, 0.5f, 0.001f); // 목표 크기
    private Vector3 startSize; // 시작 크기

    private float gravityDelay = 5f;
    private bool isCubeScaled = false;
    //    private bool isRotationChanged = false; // rotation 변경 여부 체크

    private Quaternion startRotation; // 시작 rotation
    private Quaternion targetRotation = Quaternion.identity; // 목표 rotation

    private void Start()
    {
        // 시작할 때 Cubeprefab의 크기를 저장합니다.
        startSize = cubePrefab.transform.localScale;
        startRotation = cubePrefab.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            spawning = true;
        }


        if (spawning && elapsed < duration)
        {
            int cubesToSpawn = Random.Range(cubesPerFrameMin, cubesPerFrameMax + 1);
            for (int i = 0; i < cubesToSpawn; i++)
            {
                Vector3 randomPos = new Vector3(
                    Random.Range(-spawnArea.x, spawnArea.x),
                    Random.Range(spawnArea.y - 0.3f, spawnArea.y),
                    Random.Range(-spawnArea.z, spawnArea.z)
                );
                GameObject cube = Instantiate(cubePrefab, transform.position + randomPos, Quaternion.identity);
                var cubeController = cube.AddComponent<CubeController>();
                cubeController.groundPlane = groundPlane;
                cubeController.cubeRigidbody = cube.GetComponent<Rigidbody>();
            }
            elapsed += Time.deltaTime;
        }
        else
        {
            spawning = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawning = false;
            Physics.gravity = Vector3.zero;
            foreach (var cubeController in FindObjectsOfType<CubeController>())
            {
                cubeController.cubeRigidbody.isKinematic = true;
            }

            MoveCubesUp();

        }
        
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Physics.gravity = new Vector3(0f, -9.81f * 10, 0f); // 중력을 두 배로 강하게 설정
            foreach (var cubeController in FindObjectsOfType<CubeController>())
            {
                cubeController.cubeRigidbody.isKinematic = false; // 각 큐브의 Rigidbody를 다시 움직일 수 있도록 설정
            }
            StartCoroutine(ResetGravityAfterDelay(0.5f)); // 0.5초 후에 중력을 원래대로 되돌림
        }
*/
        //L을 누르면 비의 양이 증가하도록 함
        if (Input.GetKeyDown(KeyCode.L))
        {
            cubesPerFrameMin *= 2;
            cubesPerFrameMax *= 4;
            transform.localScale *= 2;

        }
        //G키를 누르면 스파우닝이 실행되도록
        //이미 멈춰있는 큐브들도 같이 내리게 함
        if (Input.GetKeyDown(KeyCode.G))
        {
            spawning = true;
            float downSpeed = 2f;

            Physics.gravity = new Vector3(0f, -9.81f, 0f);
            foreach (var cubeController in FindObjectsOfType<CubeController>())
            {
                cubeController.cubeRigidbody.isKinematic = false;
            }
            foreach (var cube in GameObject.FindGameObjectsWithTag("Cube(Clone)"))
            {
                Rigidbody rb = cube.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // 내리는 큐브의 양을 1.2배 증가시키고자 하는 경우, 아래 라인을 수정합니다.
                    float addedForce = 1 * 0f;
                    rb.AddForce(Vector3.down * (downSpeed * addedForce), ForceMode.VelocityChange);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(ScaleCube());
            StartCoroutine(RotateCube());
            isCubeScaled = true; // 큐브가 확대/축소되었음을 표시
        }

        // 큐브가 확대/축소되고 일정 시간이 지난 후에 Rigidbody 컴포넌트를 추가
        if (isCubeScaled && elapsedTime >= gravityDelay && cubePrefab2.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = cubePrefab2.AddComponent<Rigidbody>();
        }

        // 큐브가 떨어지면 사라지도록 구현
        if (cubePrefab2.GetComponent<Rigidbody>() != null && cubePrefab2.transform.position.y < -1.7f)
        {
            Destroy(cubePrefab2);
        }

        elapsedTime += Time.deltaTime;

        void MoveCubesUp()
        {
            // Find all CubeController components in the scene
            CubeController[] cubes = FindObjectsOfType<CubeController>();

            // Move each cube up by 5 units over the course of 1 second
            foreach (CubeController cube in cubes)
            {
                StartCoroutine(MoveCubeUpCoroutine(cube.transform));
            }
        }

        void MoveCubesDown()
        {
            // Find all CubeController components in the scene
            CubeController[] cubes = FindObjectsOfType<CubeController>();

            // Move each cube down by 5 units over the course of 1 second
            foreach (CubeController cube in cubes)
            {
                StartCoroutine(MoveCubeDownCoroutine(cube.transform));
            }
        }


        IEnumerator MoveCubeUpCoroutine(Transform cubeTransform)
        {
            //1초동안 멈췄다가 올라감
            //yield return new WaitForSeconds(0.0f);

            //올라갈 시간 정하기
            float UpTime = 0.5f;
            //올라갈 거리 구하기
            float UpLength = 5f;

            float elapsedTime = 0f;
            Vector3 startPosition = cubeTransform.position;
            Vector3 targetPosition = startPosition + new Vector3(0f, UpLength, 0f);

            //선형보간법을 이용해 일정시간동안 큐브가 올라감 
            while (elapsedTime < UpTime)
            {
                // Move the cube gradually over the course of 1 second
                cubeTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / UpTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator MoveCubeDownCoroutine(Transform cubeTransform)
        {
            cubeTransform.GetComponent<Rigidbody>().isKinematic = false;

            //1초동안 멈췄다가 내려감
            //yield return new WaitForSeconds(0.0f);

            //올라갈 시간 정하기
            float DownTime = 0.5f;
            //올라갈 거리 구하기
            float DownLength = -5f;

            float elapsedTime = 0f;
            Vector3 startPosition = cubeTransform.position;
            Vector3 targetPosition = startPosition + new Vector3(0f, DownLength, 0f);

            //선형보간법을 이용해 일정시간동안 큐브가 내려감 
            while (elapsedTime < DownTime)
            {
                // Move the cube gradually over the course of 1 second
                cubeTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / DownTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator ScaleCube()
        {
            float elapsedTime1 = 0f;
            Vector3 startSize = cubePrefab2.transform.localScale;
            Vector3 startPosition = cubePrefab2.transform.position;
            Vector3 centerPosition = startPosition + new Vector3(0f, startSize.y / 2f, 0f);

            while (elapsedTime1 < duration1)
            {
                elapsedTime1 += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime1 / duration1);
                cubePrefab2.transform.localScale = Vector3.Lerp(startSize, targetSize, t);
                // 중심점을 고정시키기 위해 크기에 따라 위치를 보정합니다.
                cubePrefab2.transform.position = centerPosition - new Vector3(0f, cubePrefab2.transform.localScale.y / 2f, 0f);
                yield return null;
            }
        }

        IEnumerator RotateCube()
        {
            float elapsedTime2 = 0f;
            Quaternion startRotation = cubePrefab2.transform.rotation;

            while (elapsedTime2 < duration1)
            {
                elapsedTime2 += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime2 / duration1);
                cubePrefab2.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
                yield return null;
            }
        }

        IEnumerator ResetGravityAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Physics.gravity = new Vector3(0f, -9.81f, 0f); // 중력을 원래대로 되돌림
        }

    }



    public class CubeController : MonoBehaviour
    {
        public Transform groundPlane;
        public float speed = 2.0f;
        public Rigidbody cubeRigidbody;
        public Vector3 startSize;

        void Start()
        {
            startSize = transform.localScale;
        }

        void Update()
        {
            if (!cubeRigidbody.isKinematic)
            {
                if (transform.position.y - (startSize.y * 0.5f) <= groundPlane.position.y)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
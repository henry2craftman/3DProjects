using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("일반 설정")]
    [Tooltip("새로운 인풋 시스템입니다.")]
    [SerializeField] InputAction movement;
    [Tooltip("플레이어 비행체의 속도를 조절합니다.")]
    [SerializeField] float controlSpeed = 10;
    [Tooltip("플레이어 비행체의 회전을 조절합니다.")]
    [SerializeField] float rotationSpeed = 500;
    [SerializeField] float clampX = 10;
    [SerializeField] float clampYMax = 7;
    [SerializeField] float clampYMin = -3;
    float h, v;

    [Header("회전 설정")]
    [SerializeField] float rollFactor;
    [SerializeField] float pitchFactor;
    [SerializeField] float yawFactor;

    [Header("레이저 프리펩")]
    [SerializeField] GameObject[] lasers;

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        RotatePlayer();

        FireLaser();
    }

    private void FireLaser()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLaserActive(true);
        }
        else
        {
            SetLaserActive(false);
        }
    }

    private void SetLaserActive(bool isActive)
    {
        foreach(var laser in lasers)
        {
            var particle = laser.GetComponent<ParticleSystem>().emission;
            particle.enabled = isActive;
        }
    }

    private void RotatePlayer()
    {
        float roll = h * Time.fixedDeltaTime * rotationSpeed;
        float pitch = v * Time.fixedDeltaTime * rotationSpeed;
        float yaw = h * Time.fixedDeltaTime * rotationSpeed;

        transform.localRotation = Quaternion.Euler(transform.localRotation.x - pitch, transform.localRotation.y - yaw, transform.localRotation.z + roll);

        //float roll = transform.localPosition.x * rollFactor;
        //float pitch = transform.localPosition.y * pitchFactor;
        //float yaw = transform.localPosition.x * yawFactor;

        //transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void MovePlayer()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        float xOffset = h * Time.deltaTime * controlSpeed;
        float newXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(newXPos, -clampX, clampX);

        float yOffset = v * Time.deltaTime * controlSpeed;
        float newYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(newYPos, clampYMin, clampYMax);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }
}

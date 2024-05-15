using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //�������� ������������ � �������� 
        if (collision.gameObject.CompareTag("Target"))
        {
            //������ � ������?! ��������� ���� | ���� �� ������ �� ���� �������� ����� 3 ���
            print("hit"+collision.gameObject.name+ "!");
            Destroy(gameObject);  
        }
    }

}

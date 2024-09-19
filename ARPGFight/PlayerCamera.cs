using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPGFight
{
    public class PlayerCamera : MonoBehaviour
    {
        public Player player;

        public float distance = 10;
        public float aroundSpeed = 5;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            LookPlayer();
        }

        private void LookPlayer()
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            transform.RotateAround(player.transform.position, Vector3.up, mouseX * aroundSpeed);
            var dotResult = Vector3.Dot(transform.forward.normalized, -Vector3.up);



            if (dotResult < 0.3f)//下方镜头极限
            {
                if(mouseY > 0)
                    transform.RotateAround(player.transform.position, transform.right, mouseY * aroundSpeed / 3f);
            }
            else if (dotResult > 0.6f)//上方镜头极限
            {
                if(mouseY < 0)
                    transform.RotateAround(player.transform.position, transform.right, mouseY * aroundSpeed / 3f);
            }
            else
            {
                transform.RotateAround(player.transform.position, transform.right, mouseY * aroundSpeed / 3f);

            }

            distance = Mathf.Lerp(5, 6, dotResult  / 0.6f-0.3f);

            //Debug.Log("dotResult: "+ dotResult);

            var pos= player.transform.position + ((transform.position - player.transform.position).normalized) * distance;
            //pos.y = 3;
            transform.position = pos;
            pos = player.transform.position;
            pos.y += 1;
            pos += transform.right*0.2f;
            transform.LookAt(pos);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ARPGFight
{
    public class PlayerAni : Player
    {
        public  Animator ani;

        public float moveSpeed=3f;

        public float moveAniValue;
        public float jumpAniValue;

        //Parry
        public float parryAniValue_front;
        public float parryAniValue_back;
        public float parryAniValue_left;
        public float parryAniValue_right;

        //Attack
        public float attackAniValue_01;
        public float attackAniValue_02;
        public float attackAniValue_03;

        public int continuousAttackState = 0;

        void Start()
        {

        }

        void FixedUpdate()
        {
            Move();
        }
        private void Update()
        {
            Jump();
            JumpAniValueReduce();

            Parry();
            ParryAniValueReduce();

            Attack();
            AttackAniValueReduce();

        }


        public void Move()
        {
            bool isInputed = false;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                var aniInfo = ani.GetCurrentAnimatorStateInfo(0);
                if (aniInfo.IsName("attack") || aniInfo.IsName("attack_02") || aniInfo.IsName("attack_03"))
                    return;

                isInputed = true;

            }



            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            Rotate(vertical, horizontal);

            if(isInputed)
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            moveAniValue = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            if (moveAniValue > 0f && !isInputed)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                //Debug.Log("Sliding step");
            }

            ani.SetFloat("moveAniValue", moveAniValue);
        }

        public void Rotate(float vertical,float horizontal)
        {
            var cameraTrans = Camera.main.transform;
            bool isInputed = false;
            bool isInputedVertical = false;
            bool isInputedHorizontal = false;
            var rotation = cameraTrans.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;

            if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
            {
                isInputed = true;

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                    isInputedVertical = true;
                if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    isInputedHorizontal = true;
            }

            if (isInputedVertical && !isInputedHorizontal)
            {
                if (vertical > 0)
                {
                }
                else if (vertical < 0)
                {
                    rotation.y -= 180;
                }

            }
            else if (!isInputedVertical && isInputedHorizontal)
            {
                if (horizontal > 0)
                {
                    rotation.y += 90;
                }
                else if (horizontal < 0)
                {
                    rotation.y -= 90;

                }

            }
            else if (isInputedVertical && isInputedHorizontal)
            {
                if (vertical > 0 && horizontal > 0)
                {
                    rotation.y += 45;
                }
                else if (vertical > 0 && horizontal < 0)
                {
                    rotation.y -= 45;
                }
                else if (vertical < 0 && horizontal > 0)
                {
                    rotation.y += 135;
                }
                else if (vertical < 0 && horizontal < 0)
                {
                    rotation.y -= 135;
                }

            }





            if (isInputed)
                transform.eulerAngles = rotation;

        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Jump");
                //ani.applyRootMotion = true;
                var aniInfo = ani.GetCurrentAnimatorStateInfo(0);//获取当前动画

                if (aniInfo.IsName("attack")|| aniInfo.IsName("attack_02")|| aniInfo.IsName("attack_03"))
                {
                    return;
                }



                isTouchedGround = false;
                GetComponent<Rigidbody>().velocity = Vector3.up * 5;

                jumpAniValue = 1;
                ani.SetFloat("jumpAniValue", jumpAniValue);
            }

        }
        private void JumpAniValueReduce()
        {
            if (jumpAniValue > 0)
            {
                //var aniInfo = ani.GetCurrentAnimatorStateInfo(0);
                //Debug.Log("JumpAniValueReduce() aniInfo.normalizedTime: " + aniInfo.normalizedTime );
                if (isTouchedGround)
                {
                    //Debug.Log("jumpAniValue = 0f;");
                    jumpAniValue = 0f;
                    ani.SetFloat("jumpAniValue", jumpAniValue);

                }

            }

        }

        public bool isTouchedGround;
        private void OnCollisionEnter(Collision collision)
        {
            isTouchedGround = true;
        }


        private void Attack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Attack");

                var aniInfo = ani.GetCurrentAnimatorStateInfo(0);//获取当前动画
                
                if (aniInfo.IsName("jump"))
                {
                    return;
                }


                if (aniInfo.IsName("attack"))//second attack
                {
                    //attackAniValue_02 = 1;
                    continuousAttackState = 2;
                    //PosMoveAni(transform.forward, 0.3f);
                    //ani.SetFloat("attackAniValue_02", attackAniValue_02);

                }
                else if (aniInfo.IsName("attack_02"))//third attack
                {
                    //attackAniValue_03 = 1;
                    continuousAttackState = 3;
                    PosMoveAni(transform.forward, 0.4f);
                    //ani.SetFloat("attackAniValue_03", attackAniValue_03);

                }
                else if (aniInfo.IsName("attack_03"))
                    return;
                else//first attack
                {
                    attackAniValue_01 = 1;
                    continuousAttackState = 1;
                    PosMoveAni(transform.forward,0.2f);
                    ani.SetFloat("attackAniValue_01", attackAniValue_01);

                }


            }
        }


        /// <summary>
        /// 从攻击动画中结束
        /// </summary>
        private void AttackAniValueReduce()
        {
            if (attackAniValue_01 > 0||attackAniValue_02 > 0||attackAniValue_03 > 0)
            {
                var aniInfo = ani.GetCurrentAnimatorStateInfo(0);


                if (aniInfo.IsName("attack") && aniInfo.normalizedTime >= 1f)
                {
                    attackAniValue_01 = 0;
                    ani.SetFloat("attackAniValue_01", attackAniValue_01);

                    if (continuousAttackState == 2)
                    {
                        attackAniValue_02 = 1;
                        PosMoveAni(transform.forward, 0.3f);
                        ani.SetFloat("attackAniValue_02", attackAniValue_02);
                    }
                }
                else if (aniInfo.IsName("attack_02") && aniInfo.normalizedTime >= 1f)
                {
                    attackAniValue_02 = 0;
                    ani.SetFloat("attackAniValue_02", attackAniValue_02);

                    if (continuousAttackState == 3)
                    {
                        attackAniValue_03 = 1;
                        PosMoveAni(transform.forward, 0.4f);
                        ani.SetFloat("attackAniValue_03", attackAniValue_03);
                    }

                }
                else if (aniInfo.IsName("attack_03") && aniInfo.normalizedTime >= 1f)
                {
                    attackAniValue_03 = 0;
                    ani.SetFloat("attackAniValue_03", attackAniValue_03);

                }
            }
        }



        private void Parry()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                var aniInfo = ani.GetCurrentAnimatorStateInfo(0);//获取当前动画

                if (aniInfo.IsName("jump") || aniInfo.IsName("attack") || aniInfo.IsName("attack_02") || aniInfo.IsName("attack_03"))
                    return;

                parryAniValue_front = 1;
                ani.SetFloat("parryAniValue_front", parryAniValue_front);


                //if (Input.GetKey(KeyCode.W))
                //{
                //    parryAniValue_front = 1;
                //    ani.SetFloat("parryAniValue_front", parryAniValue_front);
                //}
                //else if (Input.GetKey(KeyCode.S))
                //{
                //    parryAniValue_back = 1;
                //    ani.SetFloat("parryAniValue_back", parryAniValue_back);
                //}
                //else if (Input.GetKey(KeyCode.A))
                //{
                //    parryAniValue_left = 1;
                //    ani.SetFloat("parryAniValue_left", parryAniValue_left);
                //}
                //else if (Input.GetKey(KeyCode.D))
                //{
                //    parryAniValue_right = 1;
                //    ani.SetFloat("parryAniValue_right", parryAniValue_right);
                //}
                //else//默认翻滚
                //{
                //    parryAniValue_front = 1;
                //    ani.SetFloat("parryAniValue_front", parryAniValue_front);
                //}
            }
        }

        private void ParryAniValueReduce()
        {
            if (parryAniValue_front > 0 || parryAniValue_back > 0 || parryAniValue_left > 0 || parryAniValue_right > 0)
            {
                var aniInfo = ani.GetCurrentAnimatorStateInfo(0);

                if (!aniInfo.IsName("parry_front") && !aniInfo.IsName("parry_back") && !aniInfo.IsName("parry_left") && !aniInfo.IsName("parry_right"))
                    return;


                if (aniInfo.normalizedTime >= 1f)
                {
                    parryAniValue_front = 0;
                    parryAniValue_back = 0;
                    parryAniValue_left = 0;
                    parryAniValue_right = 0;

                    ani.SetFloat("parryAniValue_front", parryAniValue_front);
                    ani.SetFloat("parryAniValue_back", parryAniValue_back);
                    ani.SetFloat("parryAniValue_left", parryAniValue_left);
                    ani.SetFloat("parryAniValue_right", parryAniValue_right);

                }

            }

        }



        /// <summary>
        /// position move
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        public void PosMoveAni(Vector3 direction, float distance)
        {
            transform.DOMove(transform.position+direction* distance,0.2f);
        }
    }

    

}

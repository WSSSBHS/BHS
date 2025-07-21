using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;

using Entities;
public class PlayerInputController : MonoBehaviour
{
    public Rigidbody rb;
    SkillBridge.Message.CharacterState state;       //协议当中的角色的运动状态
    public Character character;
    public float rotateSpeed = 2.0f;
    public float tureAngle = 10;
    public int speed;
    public EntityController entityController;         //实体控制器包括非玩家与玩家
    public bool onAir =false;
    // Start is called before the first frame update
    void Start()
    {
        state = SkillBridge.Message.CharacterState.Idle;
        if (this.character == null) 
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();        //协议
            cinfo.Id = 1;
            cinfo.Name = "Teat";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            this.character = new Character(cinfo);
            if (entityController != null)
            { entityController.entity = this.character; }
        }
      
    }
    private void FixedUpdate()
    {
        if (character == null)
            return;

        float v = Input.GetAxis("Vertical");   //获取前后位移的值
        if (v > 0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;     //将状态调整到运动的状态
                this.character.MoveForward();                       //改变控制角色的移动方法
                this.SendEntityEvent(EntityEvent.MoveFwd);           //调整entity的运动事件
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction)*(this.character.speed+9.81f)/100f ; //当前的值加上朝向*(基础速度+自定义速度)将结果赋予给刚体的向量
        
        }
       else if (v < -0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;     //将状态调整到运动的状态
                this.character.MoveBack();                       //改变控制角色的移动方法
                this.SendEntityEvent(EntityEvent.MoveBack);           //调整entity的运动事件
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;

        }
        else
        {
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = SkillBridge.Message.CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }


        }
        if (Input.GetButtonDown("Jump"))
        {
            this.SendEntityEvent(EntityEvent.Jump);
        }
        float h = Input.GetAxis("Horizontal");
        if (h < -0.1 || h > 0.1)
        {
            this.transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);

            if (rot.eulerAngles.y > this.tureAngle && rot.eulerAngles.y < (360 - this.tureAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            
            }
        }
    }
    Vector3 lastPos ;
    float lastSync = 0;
    private void LateUpdate()                        //每帧更新后需要做的事情
    {
        Vector3 offset = this.rb.transform.position - lastPos;                                       //这一帧的位置减去上一帧的位置得到移动的距离
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);  //magnitude在该方向上的位移 矢量
        this.lastPos =this.rb.transform.position;                                                        //将这一帧的位置覆盖到上一帧
        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 50)         //位移大于50
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));        //将现在的位置赋予给角色
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;             //更新位置
    }
    void SendEntityEvent(EntityEvent entityEvent)     //将Entity调整为当前运动状态
    {
        if (entityController != null)
        {
            entityController.OnEntityEvent(entityEvent);
        }    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

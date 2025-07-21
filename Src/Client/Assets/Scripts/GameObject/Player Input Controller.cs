using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;

using Entities;
public class PlayerInputController : MonoBehaviour
{
    public Rigidbody rb;
    SkillBridge.Message.CharacterState state;       //Э�鵱�еĽ�ɫ���˶�״̬
    public Character character;
    public float rotateSpeed = 2.0f;
    public float tureAngle = 10;
    public int speed;
    public EntityController entityController;         //ʵ���������������������
    public bool onAir =false;
    // Start is called before the first frame update
    void Start()
    {
        state = SkillBridge.Message.CharacterState.Idle;
        if (this.character == null) 
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();        //Э��
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

        float v = Input.GetAxis("Vertical");   //��ȡǰ��λ�Ƶ�ֵ
        if (v > 0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;     //��״̬�������˶���״̬
                this.character.MoveForward();                       //�ı���ƽ�ɫ���ƶ�����
                this.SendEntityEvent(EntityEvent.MoveFwd);           //����entity���˶��¼�
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction)*(this.character.speed+9.81f)/100f ; //��ǰ��ֵ���ϳ���*(�����ٶ�+�Զ����ٶ�)�������������������
        
        }
       else if (v < -0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;     //��״̬�������˶���״̬
                this.character.MoveBack();                       //�ı���ƽ�ɫ���ƶ�����
                this.SendEntityEvent(EntityEvent.MoveBack);           //����entity���˶��¼�
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
    private void LateUpdate()                        //ÿ֡���º���Ҫ��������
    {
        Vector3 offset = this.rb.transform.position - lastPos;                                       //��һ֡��λ�ü�ȥ��һ֡��λ�õõ��ƶ��ľ���
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);  //magnitude�ڸ÷����ϵ�λ�� ʸ��
        this.lastPos =this.rb.transform.position;                                                        //����һ֡��λ�ø��ǵ���һ֡
        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 50)         //λ�ƴ���50
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));        //�����ڵ�λ�ø������ɫ
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;             //����λ��
    }
    void SendEntityEvent(EntityEvent entityEvent)     //��Entity����Ϊ��ǰ�˶�״̬
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

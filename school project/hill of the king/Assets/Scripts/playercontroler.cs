using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playercontroler : Photon.PunBehaviour//플레이어 오브젝트 관련
{
    
    public int lv;//레벨
    public int exp;//경험치
    public int bullet_damage;//총알 화력
    public int team;//팀 ////punteam치기 귀찮을 때
    public Material red;//팀색깔 빨강
    public Material blue;//팀색깔 파랑
    public int bulletspeed;//총알 속도
    public GameObject vision;//시야
    public GameObject Bullet;//총알
    public bool firestate;//발사 상태 1열로 쭉나오는 거 방지
    public GameObject bullet_spawner;//총알이 생성되는 위치
    public float firedelay = 1;//연사속도
    public Text die_text;
    public float r_speed = 2;//회전 속도
    public string lastest_hit_player;//마지막으로 날 때린 플레이어의 닉네임
    public Camera cam;//1인칭카메라
    public float jump_force = 6;//점프높이
    const int MAX_Jumpcount = 1;//최대 점프 가능 횟수
    public int jumpcount = MAX_Jumpcount;
    public int max_health;//최대 체력
    public  int health;//체력
    public GameObject playeruiprefab;//플레이어ui
    public int max_magazine;//최대 탄창
    public int magazine;//현재 탄창
    public float max_reloadtime = 2;//재장전 시간
    float reload_time = 0;
    public bool reloadstate;//재장전 중인지 체크
    public static GameObject LocalplayerInstance;
    playerlv plv;
    public float speed = 3;//이동속도
    public float orginal_speed;
    public GameObject reload_text;//재장전중임을 확인시켜주는 text
    bool collide_wall;
    GameManager gameManager;
    public float RotationLimit = 80f;//rotation에 제한을 걸어둠
    public float currentRotation = 0f;//현제 rotation위치
    public playerspawner Playerspawner;
    public bool die_check;
    public struct special_ability//특수 능력
    {
       
        public float effect_time;//능력 지속 시간
        public float cool_time;//능력 재장전 시간
        public bool ability_use;//능력 사용 중
    };
    public bool ability_cool_down;//능력 재장전 중
    public float abilitty_time;
    public special_ability[] s_ability = new special_ability[5];//0=no ability 1=heart 2=blade 3= wing 4=storm
    public int s_ability_number;
 
    public int return_max_magazine()
    {
        return max_magazine;
    }
    // Start is called before the first frame update
    void Awake()//초기화
    {
        magazine = max_magazine;
        reload_text.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
        //animator = GetComponent<Animator>();
        plv = FindObjectOfType<playerlv>();
        if (!photonView.isMine)//플레이어가 내가 아니면 리턴//같이 움직이는 경우를 피하기위해
            return;
        if (photonView.isMine)//플레이어가 나일경우
        {
            Playerspawner = FindObjectOfType<playerspawner>();
            firestate = true;
            playercontroler.LocalplayerInstance = this.gameObject;
            playeruiprefab.SetActive(true);
            cam.gameObject.SetActive(true);
            //cam = GameObject.Find("Camera").GetComponent<Camera>();
            Cursor.visible = false;//커서 숨기기
            Cursor.lockState = CursorLockMode.Locked;//커서 고정
            for(int i = 0; i < s_ability.Length; i++)
            {
                switch (i)
                {
                    case 1:
                        s_ability[i].effect_time = 5;
                        s_ability[i].cool_time = 25;
                        
                        break;
                    case 2:
                        s_ability[i].effect_time = 15;
                        s_ability[i].cool_time = 20;
                        break;
                    case 3:
                        s_ability[i].effect_time = 8;
                        s_ability[i].cool_time = 15;
                        break;
                    case 4:
                        s_ability[i].effect_time = 5;
                        s_ability[i].cool_time = 30;
                        break;
                }
            }
        }
        //DontDestroyOnLoad(this.gameObject)//씬을 바꿀때 이 게임오브젝트를 파괴하지 말 것
    }
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        //팀에 따라 색깔을 바꿈
        if (GetComponent<PhotonView>() .owner.GetTeam()== PunTeams.Team.red)
        {
            GetComponent<MeshRenderer>().material = red;
        }
        if (GetComponent<PhotonView>().owner.GetTeam() == PunTeams.Team.blue)
        {
            GetComponent<MeshRenderer>().material = blue;
        }
        //게임이 끝날경우 ui 비활성화
        if (gameManager.game_set)
        {
            playeruiprefab.SetActive(false);
            reload_text.SetActive(false);
            return;
        }
        //플레이어가 죽을 때 거점을 밟고있느니 체크하는 bool값을 비활성화
        //자신말고 누가 밟고있을경우 비활성화 되자마자 바로 활성화되니 문제가 생기지 않음
        if (health <= 0)
        {
            hillofking hok = FindObjectOfType<hillofking>();
            if (team == 0)
            {
                hok.red_conquer_state = false;
            }
            else if (team == 1)
            {
                hok.blue_conquer_state = false;
            }
        }
        if (!photonView.isMine)//플레이어가 내가 아니면 리턴//같이 움직이는 경우를 피하기위해
            return;
        if (health <= 0)//죽을 때
        {
            
            hillofking hok = FindObjectOfType<hillofking>();
            //비활성화 됨을 모든 클라이언트에게 알림
            if (team == 0)
            {
                hok.GetComponent<PhotonView>().RPC("red_conquer_state_false", PhotonTargets.All);
            }
            else if (team == 1)
            {
                hok.GetComponent<PhotonView>().RPC("blue_conquer_state_false", PhotonTargets.All);
            }
            playeruiprefab.SetActive(false);
            Debug.Log("i'm die");
            GameObject can = GameObject.Find("Canvas");

            Cursor.visible = true;//
            Cursor.lockState = CursorLockMode.None;//
            //  PhotonNetwork.Destroy-> 모든 클라이언트에 이 오브젝트의 Destroy를 실행
            PhotonNetwork.Destroy(this.gameObject);
            player_get_score(-1);
            //킬로그를 생성
            boardmanager b_manager = FindObjectOfType<boardmanager>();
            b_manager.gameObject.GetComponent<PhotonView>().RPC("player_death_board", PhotonTargets.All, lastest_hit_player, this.gameObject.GetComponent<PhotonView>().owner.NickName);
            Playerspawner.max_respawn_time = 2;
            Playerspawner.playerspawned = false;
            gameManager.death++;
        }
        
           //레벨,경험치 정보를 받아옴
        GetComponent<PhotonView>().RPC("player_lv", PhotonTargets.All, plv.lv,plv.exp);
        //채력 값 제한
        if (health > max_health)
        {
            health = max_health;
        }
        collide_wall = false;
        //벽에 닿았을 때 속도 감속(너무 빨라져서 벽을 통과하는 경우 방지)
        if (collide_wall)
        {
            speed = orginal_speed / 4;
        }
        else
        {
            speed = orginal_speed;
        }
        
        if (Playerspawner.esccheck)
        {
            playeruiprefab.gameObject.SetActive(false);
        }
        else
        {
            playeruiprefab.gameObject.SetActive(true);
        }
     //탄창이 0일경우 자동 재장전
        if (magazine <= 0)
        {
            reloadstate = true;
            magazine = 0;
        }
        if (!Playerspawner.esccheck)//조작 관련
        {
            float V = Input.GetAxis("Vertical");
            float H = Input.GetAxis("Horizontal");
            float MouseX = Input.GetAxis("Mouse Y") * r_speed;
            float MouseY = Input.GetAxis("Mouse X") * r_speed;
            //마우스의 x값을 통해 플레이어가 회전
            currentRotation -= MouseX;
            currentRotation = Mathf.Clamp(currentRotation, -RotationLimit, RotationLimit);
            vision.transform.localEulerAngles = new Vector3(currentRotation, 0f, 0f);
            transform.localRotation *= Quaternion.Euler(0, MouseY, 0);
            //vision.transform.localRotation *= Quaternion.Euler(-MouseX, 0, 0);
            playermovement(H, V);
            //재장전
            if (Input.GetKeyDown(KeyCode.R))
            {
                reloadstate = true;
            }
            //총 발사
            if (Input.GetMouseButton(0))
            {
                Debug.Log("trying to make bullet0");
                if (!reloadstate && firestate)
                {
                    StartCoroutine(player_shooting());
                    playershooting();
                }
            }
            //특수 능력 사용
            if (Input.GetKeyDown(KeyCode.E))
            {
                //ability_cool_down:특수능력이 쿨다운상태인지 체크
                if (!ability_cool_down)
                    s_ability[s_ability_number].ability_use = true;
            }
            //재장전 동안의 시간 처리
            if (reloadstate)
            {
                player_reloadinng();
            }
            //스페이스 바로 점프
            if (Input.GetKeyDown(KeyCode.Space))
                playerJumping();
        }
        //특수 능력 사용시 쿨다운(재사용 대기 시간) 처리
        if (s_ability[s_ability_number].ability_use)
        {
            abilitty_time += Time.deltaTime;
            if (abilitty_time >= s_ability[s_ability_number].effect_time)//재사용 대기 시간 동안 특수능력 사용 불가
            {
                s_ability[s_ability_number].ability_use = false;
                ability_cool_down = true;
                abilitty_time = 0;
            }

        }
        if (ability_cool_down)//재사용 대기시간이 지나면 다시 사용 가능
        {
            abilitty_time += Time.deltaTime;
            if (abilitty_time >= s_ability[s_ability_number].cool_time)
            {
                ability_cool_down = false;
                abilitty_time = 0;
            }
        }
        
    }


    
    void playermovement(float x, float z)//horizontal vertical input 키로 플레이어가 이동
    {
        if(s_ability[3].ability_use)
            transform.Translate(x * speed * Time.deltaTime*2, 0, z * speed * Time.deltaTime*2);
        else
            transform.Translate(x * speed * Time.deltaTime , 0, z * speed * Time.deltaTime );
    }
    void playerJumping()//점프
    {
        Debug.Log("jump");
        if (jumpcount > 0)//점프 횟수가 남아있다면
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jump_force, ForceMode.Impulse);
            jumpcount--;
        }
    }
    IEnumerator player_shooting()//연사체크
    {
        //firedelay 간겨으로 연사
        firestate = false;
        if(s_ability[4].ability_use)// 특수능력 사용중인지 체크
            yield return new WaitForSeconds(firedelay/2);
        else
            yield return new WaitForSeconds(firedelay);
        firestate = true;
    }
    void player_reloadinng()//재장전
    {

        reload_time += Time.deltaTime;
        reload_text.SetActive(true);
        if (reload_time >= max_reloadtime)//재장전 완료
        {
            magazine = max_magazine;
            reloadstate = false;
            reload_text.SetActive(false);
            reload_time = 0;
        }
    }
    void playershooting()//총알의 화력과 속도를 동기화 한 후 발사
    {
        //PhotonNetwork.Instantiate ->모든 클라이언트에게 오브젝트가 생성
        GameObject bullet = PhotonNetwork.Instantiate(Bullet.name, bullet_spawner.transform.position, cam.gameObject.transform.rotation, 0);
        GetComponent<PhotonView>().RPC("gun_sound", PhotonTargets.All);
        //GameObject bullet =Instantiate(Bullet, bullet_spawner.transform.position, cam.gameObject.transform.rotation);
        if (s_ability[2].ability_use)
        {
            //동기화
            bullet.GetComponent<PhotonView>().RPC("set_speed", PhotonTargets.All, bulletspeed*2);
            bullet.GetComponent<PhotonView>().RPC("set_damage", PhotonTargets.All, bullet_damage*3);
        }
        else
        {
            //동기화
            bullet.GetComponent<PhotonView>().RPC("set_speed", PhotonTargets.All, bulletspeed);
            bullet.GetComponent<PhotonView>().RPC("set_damage", PhotonTargets.All, bullet_damage);
        }
        bullet.GetComponent<Rigidbody>().velocity = cam.gameObject.transform.forward * bulletspeed;
        if(!s_ability[4].ability_use)// 특수능력 사용중인지 체크 사용중일시 무한 탄창
            magazine--;
    }
    [PunRPC]
    void gun_sound()//총소리 재생
    {
        AudioSource shot_sound = bullet_spawner.GetComponent<AudioSource>();
        shot_sound.spatialBlend = 1;
        shot_sound.minDistance = 50;
        shot_sound.maxDistance = 500;
        shot_sound.Play();
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("ground"))//땅에 닿을시 점프 회복
        {
;            if (jumpcount < MAX_Jumpcount)
                jumpcount = MAX_Jumpcount;
        }
        if (col.gameObject.CompareTag("wall"))
        {
            Debug.Log("collide wall");
            collide_wall = true;
        }
            
    }
   public void player_get_score(int score)//점수 흭득
    {
        PhotonNetwork.player.SetScore(PhotonNetwork.player.GetScore() + score);
    }
    [PunRPC]
    public void player_lv(int a,int b)//레벨 동기화
    {
        lv = a;
        exp = b;
    }
    [PunRPC]
    public void player_hitting(int a,string b)//피격 동기화
    {
        Debug.Log("i'm hit!");
        if (s_ability[1].ability_use)
            health -= a * 4 / 5;
        else if (s_ability[2].ability_use)
            health -= a * 2;
        else
            health -= a;

        lastest_hit_player = b;
       
    }
    [PunRPC]
    public void player_healthup(int a)//회복 동기화
    {
        
            health += a;


    }
    [PunRPC]
    public void player_team_set(int a,int hp,int damage,float speed,int bulletspeed,float delay,float reload,int magazine,int s_ability)//플레이어 능력치 동기화
    {
        team = a;
        if (hp <= 0)
        {
            health = 10;
            max_health = 10;
        }
        else
        {
            health = hp;
            max_health = hp;
        }
        if (damage <= 0)
        {
           bullet_damage = 1;
        }
        else
        {
            bullet_damage = damage;
        }
        if (speed <= 0)
        {
            orginal_speed = 1;
            this.speed = 1;
        }
        else
        {
            orginal_speed = speed;
            this.speed = speed;
        }
        if (bulletspeed <= 0)
        {
            this.bulletspeed = 1;
        }
        else
        {
            this.bulletspeed = bulletspeed;
        }
        if (delay <= 0)
        {
            firedelay = 0.25f;
        }
        else
        {
            firedelay = delay;
        }
        if (reload <= 0)
        {
            max_reloadtime = 0.25f;
        }
        else
        {
            max_reloadtime = reload;
        }
        if (magazine <= 0)
        {
            this.magazine = 1;
            max_magazine = 1;
        }
        else
        {
            this.magazine = magazine;
            max_magazine = magazine;
        }
        s_ability_number = s_ability;
    }
    
}

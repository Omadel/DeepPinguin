using UnityEngine;

public class SwimBehaviour : MonoBehaviour {


    private void Start() {
        this.player = GameManager.Instance.Player;
        this.gameObject.SetActive(false);
    }

    private void Update() {
        Vector3 newPos = this.player.transform.position + this.moveDir * this.player.SideSwimSpeed * Time.deltaTime;
        if(newPos.x < 7f && newPos.x > .5f && newPos.z < -.5f && newPos.z > -7f) {
            this.player.transform.position = newPos;
        }
        this.player.transform.position += Vector3.down * this.player.SwimSpeed * Time.deltaTime;
    }

    public void SetDirection(string direction) {
        switch(direction) {
            case "":
                this.moveDir = Vector3.zero;
                break;
            case "Left":
                this.moveDir = new Vector3(-1, 0, -1).normalized;
                break;
            case "Right":
                this.moveDir = new Vector3(1, 0, 1).normalized;
                break;
            default:
                break;
        }
        print(direction);
    }

    private Vector3 moveDir = Vector3.zero;
    private PlayerBehaviour player = null;
}

using UnityEngine;
using System.Collections;

public class ProjectileMoveScript : MonoBehaviour {
	public Projectile data;
	public int mirror;
	public HitBoxesScript opHitBoxesScript;
	public ControlsScript opControlsScript;
	
	private Vector3 directionVector = Vector3.right;
	private float directionFloat = 1;
	private int totalHits;
	private float isHit;
	private BlockArea blockableArea;
	
	private Vector3 movement;
	
	private int opProjectileLayer;
	private int opProjectileMask;
	
	void Start () {
		gameObject.AddComponent<SphereCollider>();
		if (opControlsScript.gameObject.name == "Player1"){
			gameObject.layer = LayerMask.NameToLayer("Projectile1");
			opProjectileLayer = LayerMask.NameToLayer("Projectile2");
		}else{
			gameObject.layer = LayerMask.NameToLayer("Projectile2");
			opProjectileLayer = LayerMask.NameToLayer("Projectile1");
		}
		opProjectileMask = 1 << opProjectileLayer;

		if (mirror == -1) {
			directionVector = Vector3.left;
			directionFloat = - 1;
		}
		totalHits = data.totalHits;
		//Quaternion quaternionAngle = Quaternion.Euler(0, 0, angle) * Vector3(direction, 0, 0);
		//movement = quaternionAngle * speed;
		float angleRad = (data.directionAngle/180) * Mathf.PI;
		movement = ((Mathf.Sin(angleRad) * Vector3.up) + (Mathf.Cos(angleRad) * directionVector)) * data.speed;
		Destroy(gameObject, data.duration);
		transform.Translate(new Vector3(data.offSet.x * directionFloat, data.offSet.y, data.offSet.z));
		
		blockableArea = new BlockArea();
		blockableArea.position = gameObject.transform;
		blockableArea.radius = data.hitRadius + 3;
	}
	
	void FixedUpdate () {
		//if (myPhysicsScript.freeze) return;
		if (isHit > 0) {
			isHit -= Time.deltaTime;
			return;
		}
		transform.Translate(movement * Time.deltaTime);
		
		if (opHitBoxesScript.testCollision(blockableArea) != Vector3.zero) {
			opControlsScript.CheckBlocking(true);
		}

		if (data.projectileCollision){
			RaycastHit raycastHit = new RaycastHit();
			if (Physics.SphereCast(transform.position, data.hitRadius * (data.speed * .1f), Vector3.forward, out raycastHit, 1, opProjectileMask)) {
				if (data.impactPrefab != null){
					GameObject hitEffect = (GameObject) Instantiate(data.impactPrefab, transform.position, Quaternion.Euler(0,0,data.directionAngle));
					Destroy(hitEffect, 1);
				}
				totalHits --;
				if (totalHits <= 0){
					Destroy(gameObject);
				}
				isHit = .3f;
				transform.Translate(movement * -1 * Time.deltaTime);
			}
		}

		if (opHitBoxesScript.testCollision(transform.position, data.hitRadius)) {
			//Vector3 newPos = transform.position + Vector3.back;
			if (data.impactPrefab != null){
				GameObject hitEffect = (GameObject) Instantiate(data.impactPrefab, transform.position, Quaternion.Euler(0,0,data.directionAngle));
				Destroy(hitEffect, 1);
			}
			totalHits --;
			if (totalHits <= 0){
				Destroy(gameObject);
			}

			isHit = opControlsScript.GetFreezingTime(data.hitStrengh) * 1.2f;
			Hit hit = new Hit();
			hit.hitType = data.hitType;
			hit.hitStrengh = data.hitStrengh;
			hit.hitStunType = HitStunType.Frames;
			hit.hitStunOnHit = data.hitStunOnHit;
			hit.hitStunOnBlock = data.hitStunOnBlock;
			hit.damageOnHit = data.damageOnHit;
			hit.damageType = data.damageType;
			hit.resetPreviousHorizontalPush = true;
			hit.pushForce = data.pushForce;
			hit.pullEnemyIn = new PullIn();
			hit.pullEnemyIn.enemyBodyPart = BodyPart.none;
			
			if (!opControlsScript.stunned && opControlsScript.isBlocking && opControlsScript.TestBlockStances(hit.hitType)){
				opControlsScript.GetHitBlocking(hit, 20, transform.position);
			}else if (opControlsScript.potentialParry > 0 && opControlsScript.TestParryStances(hit.hitType)){
				opControlsScript.GetHitParry(hit, transform.position);
			}else{
				opControlsScript.GetHit(hit, 30, Vector3.zero);
			}
			
			opControlsScript.CheckBlocking(false);
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, data.hitRadius);
		
		Gizmos.color = Color.blue;
		Vector3 blockableAreaPosition = blockableArea.position.position;
		if (!UFE.config.detect3D_Hits) blockableAreaPosition.z = -1;
		blockableAreaPosition.x += (blockableArea.radius/2) * directionFloat;
		Gizmos.DrawWireSphere(blockableAreaPosition, blockableArea.radius);
    }
}

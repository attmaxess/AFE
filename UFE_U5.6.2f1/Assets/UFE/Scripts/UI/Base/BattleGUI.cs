using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleGUI : UFEScreen {
	#region public class definitions
	[Serializable]
	public class PlayerInfo{
		public CharacterInfo character;
		public float targetLife;
		public float totalLife;
		public int wonRounds;
	}
	#endregion

	#region protected instance properties
	protected PlayerInfo player1 = new PlayerInfo();
	protected PlayerInfo player2 = new PlayerInfo();
	protected bool isRunning;
	#endregion

	#region public override methods
	public override void DoFixedUpdate(){
		base.DoFixedUpdate();
	}

	public override void OnShow (){
		base.OnShow ();

		/* Subscribe to UFE events:
		/* Possible Events:
		 * OnLifePointsChange(float newLifePoints, CharacterInfo player)
		 * OnNewAlert(string alertMessage, CharacterInfo player)
		 * OnHit(MoveInfo move, CharacterInfo hitter)
		 * OnMove(MoveInfo move, CharacterInfo player)
		 * OnRoundEnds(CharacterInfo winner, CharacterInfo loser)
		 * OnRoundBegins(int roundNumber)
		 * OnGameEnds(CharacterInfo winner, CharacterInfo loser)
		 * OnGameBegins(CharacterInfo player1, CharacterInfo player2, StageOptions stage)
		 * 
		 * usage:
		 * UFE.OnMove += YourFunctionHere;
		 * .
		 * .
		 * void YourFunctionHere(T param1, T param2){...}
		 * 
		 * The following code bellow show more usage examples
		 */

        // Global Events
		UFE.OnGameBegin += this.OnGameBegin;
		UFE.OnGameEnds += this.OnGameEnd;
		UFE.OnGamePaused += this.OnGamePaused;
		UFE.OnRoundBegins += this.OnRoundBegin;
		UFE.OnRoundEnds += this.OnRoundEnd;
		UFE.OnLifePointsChange += this.OnLifePointsChange;
		UFE.OnNewAlert += this.OnNewAlert;
        UFE.OnHit += this.OnHit;
        UFE.OnBlock += this.OnBlock;
		UFE.OnParry += this.OnParry;
		UFE.OnMove += this.OnMove;
		UFE.OnBasicMove += this.OnBasicMove;
		UFE.OnButton += this.OnButtonPress;
		UFE.OnTimer += this.OnTimer;
		UFE.OnTimeOver += this.OnTimeOver;
		UFE.OnInput += this.OnInput;

        // Move Events
        UFE.OnBodyVisibilityChange += this.OnBodyVisibilityChange;
        UFE.OnParticleEffects += this.OnParticleEffects;
	}

	public override void OnHide (){
		UFE.OnGameBegin -= this.OnGameBegin;
		UFE.OnGameEnds -= this.OnGameEnd;
		UFE.OnGamePaused -= this.OnGamePaused;
		UFE.OnRoundBegins -= this.OnRoundBegin;
		UFE.OnRoundEnds -= this.OnRoundEnd;
		UFE.OnLifePointsChange -= this.OnLifePointsChange;
		UFE.OnNewAlert -= this.OnNewAlert;
        UFE.OnHit -= this.OnHit;
        UFE.OnBlock -= this.OnBlock;
        UFE.OnParry -= this.OnParry;
        UFE.OnMove -= this.OnMove;
        UFE.OnBasicMove -= this.OnBasicMove;
        UFE.OnButton -= this.OnButtonPress;
		UFE.OnTimer -= this.OnTimer;
		UFE.OnTimeOver -= this.OnTimeOver;
		UFE.OnInput -= this.OnInput;

        UFE.OnBodyVisibilityChange -= this.OnBodyVisibilityChange;
        UFE.OnParticleEffects -= this.OnParticleEffects;

		base.OnHide ();
	}
	#endregion

	#region protected instance methods
	protected virtual void OnGameBegin(CharacterInfo player1, CharacterInfo player2, StageOptions stage){
		this.player1.character = player1;
		this.player1.targetLife = player1.lifePoints;
		this.player1.totalLife = player1.lifePoints;
		this.player1.wonRounds = 0;

		this.player2.character = player2;
		this.player2.targetLife = player2.lifePoints;
		this.player2.totalLife = player2.lifePoints;
		this.player2.wonRounds = 0;

		AudioClip clip = this.GetStageMusic(stage);
		if (clip != null){
			UFE.PlayMusic(clip);
		}

		this.isRunning = true;
	}

	protected virtual void OnGameEnd(CharacterInfo winner, CharacterInfo loser){
		this.isRunning = false;

		if (UFE.gameMode == GameMode.VersusMode
            || UFE.gameMode == GameMode.NetworkGame){
			UFE.StartVersusModeAfterBattleScreen();
		}else if (UFE.gameMode == GameMode.StoryMode){
			if (winner == this.player1.character){
				UFE.WonStoryModeBattle();
			}else{
				UFE.StartStoryModeContinueScreen();
			}
		}else{
			UFE.StartMainMenuScreen();
		}
	}

	protected virtual void OnGamePaused(bool isPaused){

	}

	protected virtual void OnRoundBegin(int roundNumber){
		
	}

	protected virtual void OnRoundEnd(CharacterInfo winner, CharacterInfo loser){
		//++this.player1WonRounds;
		//++this.playe21WonRounds;
	}

    protected virtual void OnLifePointsChange(float newFloat, CharacterInfo player) {
        // You can use this to have your own custom events when a player's life points changes
        // player.playerNum = 1 or 2
	}

    protected virtual void OnNewAlert(string msg, CharacterInfo player) {
        // You can use this to have your own custom events when a new text alert is fired from the engine
        // player.playerNum = 1 or 2
	}

	protected virtual void OnHit(HitBox strokeHitBox, MoveInfo move, CharacterInfo player){
        // player.playerNum = 1 or 2
		// You can use this to have your own custom events when a character gets hit
	}

    protected virtual void OnBlock(HitBox strokeHitBox, MoveInfo move, CharacterInfo player) {
        // You can use this to have your own custom events when a player blocks.
        // player.playerNum = 1 or 2
        // player = character blocking
    }

    protected virtual void OnParry(HitBox strokeHitBox, MoveInfo move, CharacterInfo player) {
        // You can use this to have your own custom events when a character parries an attack
        // player.playerNum = 1 or 2
        // player = character parrying
    }

    protected virtual void OnMove(MoveInfo move, CharacterInfo player) {
        // Fires when a player successfully executes a move
        // player.playerNum = 1 or 2
	}

    protected virtual void OnBasicMove(BasicMoveReference basicMove, CharacterInfo player) {
        // Fires when a player successfully executes a move
        // player.playerNum = 1 or 2
    }

    protected virtual void OnButtonPress(ButtonPress buttonPress, CharacterInfo player) {
        // Fires when a player successfully executes a move
        // player.playerNum = 1 or 2
    }

    protected virtual void OnBodyVisibilityChange(MoveInfo move, CharacterInfo player, BodyPartVisibilityChange bodyPartVisibilityChange, HitBox hitBox) {
        // Fires when a move casts a body part visibility change
        // player.playerNum = 1 or 2
    }

    protected virtual void OnParticleEffects(MoveInfo move, CharacterInfo player, MoveParticleEffect particleEffects) {
        // Fires when a move casts a particle effect
        // player.playerNum = 1 or 2
    }

	protected virtual void OnTimer(float time){

	}

	protected virtual void OnTimeOver(){
		
	}

	protected virtual void OnInput(InputReferences[] inputReferences, int player){

	}
	#endregion

	#region protected instance methods
	protected virtual AudioClip GetStageMusic(StageOptions stage){
		if (UFE.config.stageMusicStorage == StorageMode.Legacy) {
			return stage.music;
		}else{
#if !UFE_BASIC
			return Resources.Load<AudioClip> (stage.musicResourcePath);
#endif
		}
	}
	#endregion
}

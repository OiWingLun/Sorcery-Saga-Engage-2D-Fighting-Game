using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Walk(float speed) {
        anim.SetFloat(AnimationTags.Walk, Mathf.Abs(speed));
    }
    public void Jump(bool isJumping) {
        anim.SetBool(AnimationTags.Jumping_Bool, isJumping);
    }
    public void Punch1() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Punch1_Trigger);
    }
    public void Punch2() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Punch2_Trigger);
    }
    public void Kick1() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Kick1_Trigger);
    }
    public void Kick2() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Kick2_Trigger);
    }
    public void Special1() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Special1_Trigger);
    }
    public void Special2() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Special2_Trigger);
    }
    public void Special3() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Special2_Trigger);
    }
    public void SwaySword3() {
    AudioController.audioController.PlaySound("Punch");
    anim.SetTrigger(AnimationTags.SwaySword3_Trigger);
    Invoke(nameof(PlayDelayedSound), 0.7f);
    }
    private void PlayDelayedSound() {
    AudioController.audioController.PlaySound("Punch");
    }
    public void SwaySword4() {
        AudioController.audioController.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.SwaySword4_Trigger);
    }
    public void JumpAttack1(bool jumpAttack1) {
        AudioController.audioController.PlaySound("Punch");
        anim.SetBool(AnimationTags.JumpAttack1_Bool, jumpAttack1);
    }
    public void JumpAttack2(bool jumpAttack2) {
        AudioController.audioController.PlaySound("Punch");
        anim.SetBool(AnimationTags.JumpAttack2_Bool, jumpAttack2);
    }
    public void JumpAttack3(bool jumpAttack3) {
        AudioController.audioController.PlaySound("Punch");
        anim.SetBool(AnimationTags.JumpAttack3_Bool, jumpAttack3);
    }
    public void Crouch(bool isCrouching) {
        anim.SetBool(AnimationTags.Crouch_Trigger, isCrouching);
    }
    public void CrouchAttack1(bool crouchAttack1)
    {
        anim.SetBool(AnimationTags.CrouchAttack1_Bool, crouchAttack1);
    }
    public void CrouchAttack2(bool crouchAttack2)
    {
        anim.SetBool(AnimationTags.CrouchAttack2_Bool, crouchAttack2);
    }
    public void Hurt() {
        anim.SetTrigger(AnimationTags.Hurt_Trigger);
    }
    public void Block(bool isBlock) {
        anim.SetBool(AnimationTags.Block_Trigger, isBlock);
    }
    public void Die(bool isDie) {
        anim.SetBool(AnimationTags.Die_Bool, isDie);
    }
    internal object GetCurrentAnimatorStateInfo(int v)
    {
        throw new NotImplementedException();
    }
}

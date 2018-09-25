﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public BulletData data;
	Rigidbody2D self;

    #region UnityFunctions
    void Start ()
	{
        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = data.bulletColor;
        renderer.sprite = data.sprite;
        self = GetComponent<Rigidbody2D>();
		self.velocity = transform.up * data.speed;
		self.mass = data.mass;
        if(data.trail != null)
        {
            Instantiate(data.trail, transform);
        }
        if (data.hasLifeTime)
        {
            var component = gameObject.AddComponent<SelfDestruct>();
            component.lifeTime = data.lifeTime;
        }
        if(data.isHoming)
        {
            var component = gameObject.AddComponent<HomingBehavior>();

            component.taggedToTarget = data.taggedToTarget;
            component.targetingRadius = data.targetingRadius;
            component.homingRefreshRate = data.homingRefreshRate;
            component.speed = data.speed;
            component.rotateSpeed = data.rotateSpeed;
    
        }
    }
	void OnCollisionEnter2D(Collision2D coll)
	{
		foreach (var tag in data.taggedToDamage)
		{
			if (tag == coll.gameObject.tag)
			{
				coll.gameObject.GetComponent<Health>().TakeDamage(data.damage, gameObject);
			}
		}
		foreach (var tag in data.taggedToDestroy)
		{
			if (tag == coll.gameObject.tag)
			{
				if(data.impactAnim != null)
                {
                    Instantiate(data.impactAnim, transform.position, Quaternion.identity);
                }
                AudioManager.Instance.PlaySound(data.impactAudio);
				Destroy(gameObject);
			}
		}
		foreach (var tag in data.taggedToDestroyWithoutEffects)
		{
			if (tag == coll.gameObject.tag)
			{
				Destroy(gameObject);
			}
		}
	}
    #endregion
}

using UnityEngine;
using System.Collections;

public class BaseClass {

	//Attributes
	public int Level{ get; set; }
	public int Health{ get; set; }
	public int Resource{ get; set; }
	public int CurrentHealth{ get; set; }
	public int CurrentResource{ get; set; }
	public string Name{ get; set; }
	
	public int Strength{ get; set; }
	public int Defense{ get; set; }
	public float Accuracy{ get; set; }
	public float Speed{ get; set; }
	public int Wisdom{ get; set; }
	public int Dexterity{ get; set; }
	public float HPRate{ get; set; }
	public float MPRate{ get; set; }

	public int Experience{ get; set; }


	//Methods
	public void TakeDamage(int damage)
	{
		CurrentHealth -= damage;
	}

	public void HealthRegeneration()
	{
		if (CurrentHealth < Health)
		{
			CurrentHealth += (int)System.Math.Round(CurrentHealth*HPRate);
			if (CurrentHealth >= Health)
			{
				CurrentHealth = Health;
			}
		}

		else
		{
			CurrentResource=Resource;
		}

	}

	public void ResourceRegeneration()
	{
		if (CurrentResource < Health)
		{
			CurrentResource += (int)System.Math.Round(CurrentResource*MPRate);
			if (CurrentResource >= Resource)
			{
				CurrentResource = Resource;
			}
		}

		else
		{
			CurrentResource=Resource;
		}
	}
}

using UnityEngine;
using System.Collections;

public class RandomShake : MonoBehaviour {
	
	public float duration = 0.5f;
	public float magnitude = 0.1f;
    public static RandomShake randomShake;

    void Awake()
    {
        randomShake = this;
    }

	// -------------------------------------------------------------------------
	public void PlayShake() 
    {
        StopAllCoroutines();
		StartCoroutine("Shake");
	}
    public void PlaySinShake()
    {
        StopAllCoroutines();
        StartCoroutine("SinShake");
    }
    public void PlayShakeX()
    {
        StopAllCoroutines();
        StartCoroutine("ShakeX");
    }
    // -------------------------------------------------------------------------
    IEnumerator SinShake()
    {
        //Debug.Log("sinshake");
        float elapsed = 0.0f;

        Vector3 originalCamPos = gameObject.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = Mathf.Clamp(Mathf.Sin(percentComplete * Mathf.PI), 0.0f, 1.0f);

            // map noise to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            gameObject.transform.position = originalCamPos + CameraLogic.camLogic.moveVel * elapsed + new Vector3(x, y, 0.0f);

            yield return null;
        }
        gameObject.transform.position = originalCamPos + CameraLogic.camLogic.moveVel * elapsed;
    }
    // -------------------------------------------------------------------------
    IEnumerator ShakeX()
    {
        float elapsed = 0.0f;

        Vector3 originalCamPos = gameObject.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map noise to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            x *= magnitude*5.0f * damper;

            gameObject.transform.position = originalCamPos + CameraLogic.camLogic.moveVel * elapsed + new Vector3(x, 0.0f, 0.0f);

            yield return null;
        }
        gameObject.transform.position = originalCamPos + CameraLogic.camLogic.moveVel * elapsed;
    }	

	// -------------------------------------------------------------------------
	IEnumerator Shake() {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = gameObject.transform.position;
		
		while (elapsed < duration) 
        {
			elapsed += Time.deltaTime;
			
			float percentComplete = elapsed / duration;			
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map noise to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

            gameObject.transform.position = originalCamPos + CameraLogic.camLogic.moveVel * elapsed + new Vector3(x, y, 0.0f);
				
			yield return null;
		}
        gameObject.transform.position = originalCamPos + CameraLogic.camLogic.moveVel * elapsed;
	}
}

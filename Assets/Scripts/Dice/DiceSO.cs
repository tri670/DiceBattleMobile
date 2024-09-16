using GameControl;
using UnityEngine;

namespace Dice
{
    [CreateAssetMenu(fileName = "Dice", menuName = "Dice/Dice", order = 1)]
    public class DiceSO : ScriptableObject
    {
        public Rigidbody rb;
        public GameObject model;

        [SerializeField] private GameObject modelPrefab;
        [SerializeField] private DiceFaceSO topFaceSo;
        [SerializeField] private DiceFaceSO bottomFaceSo;
        [SerializeField] private DiceFaceSO frontFaceSo;
        [SerializeField] private DiceFaceSO backFaceSo;
        [SerializeField] private DiceFaceSO leftFaceSo;
        [SerializeField] private DiceFaceSO rightFaceSo;

        private bool _isRolling;
        private bool _isIntoAction;
        private DiceFaceSO _activeDaceFaceSo;

        private ParticleSystem _attackEffect;
        private ParticleSystem _armorEffect;
        private ParticleSystem _healEffect;


        public void Spawn(Transform parent, GameController gameController)
        {
            model = Instantiate(modelPrefab, parent, false);
            rb = model.GetComponent<Rigidbody>();

            _attackEffect = model.transform.Find("AttackEffect").GetComponent<ParticleSystem>();
            _armorEffect = model.transform.Find("ArmorEffect").GetComponent<ParticleSystem>();
            _healEffect = model.transform.Find("HealEffect").GetComponent<ParticleSystem>();

            if (model.GetComponent<Collider>() == null)
            {
                model.AddComponent<BoxCollider>();
            }

            model.AddComponent<DiceClickHandler>().Initialize(this, gameController);

            if (_attackEffect == null || _armorEffect == null || _healEffect == null)
            {
                Debug.LogError("Some dice VFX not founded");
            }
        }


        public Quaternion GetRotationForActiveFace()
        {
            switch (_activeDaceFaceSo)
            {
                case var face when face == topFaceSo:
                    return Quaternion.Euler(0, 0, 0);
                case var face when face == bottomFaceSo:
                    return Quaternion.Euler(0, 180, 180);
                case var face when face == frontFaceSo:
                    return Quaternion.Euler(270, 0, 180);
                case var face when face == backFaceSo:
                    return Quaternion.Euler(90, 0, 0);
                case var face when face == rightFaceSo:
                    return Quaternion.Euler(0, 90, 90);
                case var face when face == leftFaceSo:
                    return Quaternion.Euler(0, 270, 270);
                default:
                    return model.transform.rotation;
            }
        }

        public void Rotate(Vector3 rotation, float jumpForce)
        {
            if (rb == null)
            {
                Debug.LogError("Rigidbody not founded.");
                return;
            }

            rb.AddTorque(rotation, ForceMode.Impulse);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        public void ApplyDiceAction(Character.Character source, Character.Character target)
        {
            if (_activeDaceFaceSo == null)
            {
                Debug.LogError("Active dice face is null.");
                return;
            }

            if (_activeDaceFaceSo.isAttack)
            {
                target.GetDamage(_activeDaceFaceSo.damagePoint);
                _attackEffect.Play();
            }

            if (_activeDaceFaceSo.isArmorUp)
            {
                source.IncreaseArmor(_activeDaceFaceSo.armorPoints);
                _armorEffect.Play();
            }

            if (_activeDaceFaceSo.isHeal)
            {
                source.RestoreHealth(_activeDaceFaceSo.healPoint);
                _healEffect.Play();
            }

            if (_activeDaceFaceSo.isPoison)
            {
                target.AddPoisonPoints(_activeDaceFaceSo.poisonPoints);
                _healEffect.Play();
            }
        }

        public void DetectUpFace()
        {
            Vector3[] cubeDirections =
            {
                model.transform.up,
                -model.transform.up,
                model.transform.right,
                -model.transform.right,
                model.transform.forward,
                -model.transform.forward
            };

            DiceFaceSO[] diceFaces =
                { topFaceSo, bottomFaceSo, rightFaceSo, leftFaceSo, frontFaceSo, backFaceSo };

            float minAngle = float.MaxValue;
            int closestFaceIndex = 0;

            for (int i = 0; i < cubeDirections.Length; i++)
            {
                float angle = Vector3.Angle(cubeDirections[i], Vector3.up);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    closestFaceIndex = i;
                }
            }

            _activeDaceFaceSo = diceFaces[closestFaceIndex];
        }
    }
}
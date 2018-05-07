using UnityEngine;

namespace PC2D
{
    /// <summary>
    /// Este es un ejemplo muy muy simple de cómo un sistema de animación puede consultar información del motor para establecer el estado.
    /// Esto se puede hacer para reproducir estados explícitamente, como se muestra a continuación, o enviar disparadores, flotantes o bools al animador. Lo más probable es esto
    /// tendrá que escribirse para adaptarse a las necesidades de su juego.
    /// </summary>

    public class PlatformerAnimation2D : MonoBehaviour
    {

        public float jumpRotationSpeed;  //Velocidad que dara los giros al saltar 
        public GameObject visualChild;   //Objeto al cual se va a animar

        private PlatformerMotor2D _motor; //instacia de PlatfomerMoto2D
        private Animator _animator;  //Instacia al objeto Animator
        private bool _isJumping; //variable la cual le dice si esta en salto o no
        private bool _currentFacingLeft; //Dash

        // Iniciaciones al arancar 
        void Start()
        {
             //Capturar todos los componentes PlatformerMotor(El cual )
            _motor = GetComponent<PlatformerMotor2D>();
            //Esta parte es para poder insertar animaciones al objeto que se a intrudicodo 
            _animator = visualChild.GetComponent<Animator>();
            //Play, toma el nombre de una animacion para que sea la primera que muestre a comenzar 
            _animator.Play("Idle");
			//No se aun 
            _motor.onJump += SetCurrentFacingLeft;
        }

        // Update, todo codigo escrito aqui pasara cada cuadro "fps"
        void Update()
        {
			//Este if verifica si el personaje realiza un salto, si este es asì le inserta la animacion de "Jump" luego compara si se realizo el salto 
			//Adelante o atras para guardar en una variable float
			//Luego en RotateDir vuelve a hacer otra comparacion para saber de que lador hara el giros
            if (_motor.motorState == PlatformerMotor2D.MotorState.Jumping ||
                _isJumping &&
                    (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
            {
                _isJumping = true;
                _animator.Play("Jump");

                if (_motor.velocity.x <= -0.1f)
                {
                    _currentFacingLeft = true;
                }
                else if (_motor.velocity.x >= 0.1f)
                {
                    _currentFacingLeft = false;
                }

                Vector3 rotateDir = _currentFacingLeft ? Vector3.forward : Vector3.back;
                visualChild.transform.Rotate(rotateDir, jumpRotationSpeed * Time.deltaTime);
            }
			//Else que verifica cual es el estado exacto del player (Proceso que hace comparando con el script PlatfomerMoto2D)
			//Basicamente aquì pone toda las animaciones
            else
            {
                _isJumping = false;
                visualChild.transform.rotation = Quaternion.identity;

                if (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast)
                {
                    _animator.Play("Fall");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.WallSliding ||
                         _motor.motorState == PlatformerMotor2D.MotorState.WallSticking)
                {
                    _animator.Play("Cling");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.OnCorner)
                {
                    _animator.Play("On Corner");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping)
                {
                    _animator.Play("Slip");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Dashing)
                {
                    _animator.Play("Dash");
                }
                else
                {
                    if (_motor.velocity.sqrMagnitude >= 0.1f * 0.1f)
                    {
                        _animator.Play("Walk");
                    }
                    else
                    {
                        _animator.Play("Idle");
                    }
                }
            }

            // Facing
            float valueCheck = _motor.normalizedXMovement;

            if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping ||
                _motor.motorState == PlatformerMotor2D.MotorState.Dashing ||
                _motor.motorState == PlatformerMotor2D.MotorState.Jumping)
            {
                valueCheck = _motor.velocity.x;
            }
            
            if (Mathf.Abs(valueCheck) >= 0.1f)
            {
                Vector3 newScale = visualChild.transform.localScale;
                newScale.x = Mathf.Abs(newScale.x) * ((valueCheck > 0) ? 1.0f : -1.0f);
                visualChild.transform.localScale = newScale;
            }
        }

        private void SetCurrentFacingLeft()
        {
            _currentFacingLeft = _motor.facingLeft;
        }
    }
}

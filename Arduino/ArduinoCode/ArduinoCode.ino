#include <MPU6050.h>
#include <I2Cdev.h>
#include <SoftwareSerial.h>
#include <Wire.h>


/*
 * FUNCTION GIVED BY THE LIBRARY
 * THIS IS USED TO SET THE BLUETOOTH PORT SERIAL
 * 
 * PARAMETERS:RX PIN, TX PIN
 * 
 */
SoftwareSerial BlueSerial (2,3);

/*
 * 
 * DEFINE THE MPU 6050 SENSOR THAT IS GOING TO BE USED
 * BY DEFAULT DIRECTION IS 0X68. MPU6050 ONLY WORKS WITH 0X68 AND 0X69
 * IN CASE YOU WANT TO USE 0X69 YOU SHOULD PASS IT AS PARAMETER
 * F.ex: Mpu6050 sensor(0x69);
 * 
 */
 
MPU6050 sensor;

/*
 * DECLARE VARIABLES
 * ACCELERATION
 */

int ax;
int ay;
int az;

/*
 * DECLARE VARIABLES
 * ROTATION
 */
int gx;
int gy;
int gz;

/*
 * DECLARE VARIABLE
 * PREVIOUS TIME
 */
 
long tiempo_prev;

/*
 * DECLARE VARIABLE
 * PREVIOUS DISPLACEMENT
 */
 float desp_acc_x_prev;
 float desp_acc_y_prev;
 float desp_acc_z_prev;

 float desp_acc_x_prev_o;
 float desp_acc_y_prev_o;
 float desp_acc_z_prev_o;
 
 float vo_x;
 float vo_y;
 float vo_z;
 
 float vo_x_prev;
 float vo_y_prev;
 float vo_z_prev;

 float ax_m_s2_prev;
 float ay_m_s2_prev;
 float az_m_s2_prev;
 
 float ax_m_s2_prev_o;
 float ay_m_s2_prev_o;
 float az_m_s2_prev_o;
 
 float v_x_prev;
 float v_y_prev;
 float v_z_prev;
/*
 * DECLARE VARIABLE
 * ROTATION ANGLES
 */
float girosc_ang_x, girosc_ang_y;
float gyro_ang_x_prev, gyro_ang_y_prev;

float dt;
float dt_prev;
/*
 * DECLARE VARIABLES
 * FOR ROTATION ANGLE WITH COMPLEMENTARY FILTER
 */
float ang_x, ang_y;
float ang_x_prev, ang_y_prev;

/*
 * DECLARE VARIABLES
 * TO STORE OFFSETS
 */
int acc_x_off;
int acc_y_off;
int acc_z_off;

int  gyro_x_off;
int  gyro_y_off;
int gyro_z_off;
/*
 * DECLARE VARIABLES 
 * VARIABLES USED FOR THE FILTER
 */

long f_ax,f_ay, f_az;
int p_ax, p_ay, p_az;
long f_gx,f_gy, f_gz;
int p_gx, p_gy, p_gz;
int counter=0;


 
/*
 * DECLARE VARIABLES 
 * STORE IF THE SENSOR IS READY
 */
int ready;

void setup()
{      
  /* START SERIAL COMUNICATION WITH BLUETOOTH
  * 
  * PARAMETERS: INT BAUD RATE
  * 
  * IN CASE YOU DON'T KNOW WICH BAUD RATE CORRESPONDS TO YOUR COMPONENT LOOK AT IT'S DOCUMENTATION
  * 
  */
  
  BlueSerial.begin(9600); 
  Serial.begin(9600);
  /* START I2C */
  Wire.begin();

  /* INITIALIZE MPU6050 SENSOR */
  sensor.initialize();  

  /*
   * SET PREVIOUS TIME AT ACTUAL TIME IN ORDER TO MAKE THE NEEDED CALCULATIONS AT LOOP
   */
    dt_prev=millis();
    tiempo_prev=millis();
Serial.println("Comencemos");
 /*SENT READY TO FALSE*/
  int ready=0;
  
  /* CALL CALIBRATE_SENSOR FUNCTION */
  calibrate_sensors();  

  /* TEST THAT THE SENSOR CONNECTION IS WORKING PROPERLY */
   if (sensor.testConnection()) Serial.println("Sensor iniciado correctamente");
  else Serial.println("Error al iniciar el sensor");
}
 

 


 void loop()
{ 
 
    
  
  /*GET ACCELERATION AND ROTATION FROM MPU6050 SENSOR*/
  sensor.getAcceleration(&ax, &ay, &az);
  sensor.getRotation(&gx, &gy, &gz);

 /* 
  *  ESCALE LECTURES FROM SENSOR AND TURN VALUE INTO ANGULAR VELOCITY
  *  VALUES ARE STORED IN DEGREES PER SECOND
  */

  float gx_deg_s = gx * (250.0/32768.0); 
  float gy_deg_s = gy * (250.0/32768.0); 
  float gz_deg_s = gz * (250.0/32768.0); 
  
 /* 
  *  ESCALE LECTURES FROM SENSOR AND TURN VALUE INTO ACCELERATION
  *  VALUES ARE STORED IN METERS PER SECOND SQUARED
  */
  float ax_m_s2 = ax * (9.81/16384.0); 
  float ay_m_s2 = ay * (9.81/16384.0); 
  float az_m_s2 = az * (9.81/16384.0); 

 /*
  * GET ACTUAL ANGLE
  */
 dt = (millis()-tiempo_prev)/1000.0;
  tiempo_prev=millis();
  
/*
 * CALCULATE THE ANGLES WITH THE ACCELEROMETER
 */
  float accel_ang_x=atan(ay/sqrt(pow(ax,2) + pow(az,2)))*(180.0/3.14);
  float accel_ang_y=atan(-ax/sqrt(pow(ay,2) + pow(az,2)))*(180.0/3.14);
  
  /*
   * GET ROTATION ANGLE WITH COMPLEMENTARY FILTER 
   */
   
  ang_x = 0.98*(gyro_ang_x_prev+(gx/131)*dt) + 0.02*accel_ang_x;
  ang_y = 0.98*(gyro_ang_y_prev+(gy/131)*dt) + 0.02*accel_ang_y;
  
  
  gyro_ang_x_prev=ang_x;
  gyro_ang_y_prev=ang_y;

/*
 * CALCULATE THE DISPLACEMENT 
 */
float a_x_average=(ax_m_s2 + ax_m_s2_prev)  / 2;
float a_y_average=(ay_m_s2 + ay_m_s2_prev)  / 2;
float a_z_average=(az_m_s2 + az_m_s2_prev)  / 2;

float v_x_present = v_x_prev + a_x_average * dt ;
float v_x_average = (v_x_prev + v_x_present) / 2;

float v_y_present = v_y_prev + a_y_average * dt ;
float v_y_average = (v_y_prev + v_y_present) / 2;

float v_z_present = v_z_prev + a_z_average * dt ;
float v_z_average = (v_z_prev + v_z_present) / 2;



/* INSTANTANEOUS VELOCITY */
 vo_x=1/2*(ax_m_s2_prev+ax_m_s2_prev_o)*dt_prev+vo_x_prev;
 vo_y=1/2*(ay_m_s2_prev+ay_m_s2_prev_o)*dt_prev+vo_y_prev;
 vo_z=1/2*(az_m_s2_prev+az_m_s2_prev_o)*dt_prev+vo_z_prev;


/*DISPLACEMENT*/
 float desp_acc_x=v_x_prev*pow(dt,2)+(vo_x*dt)*a_x_average*dt;
 float desp_acc_y=v_y_prev*pow(dt,2)+(vo_x*dt)*a_y_average*dt;
 float desp_acc_z=v_z_prev*pow(dt,2)+(vo_x*dt)*a_z_average*dt;

/*SET PREVIOUS DISPLACEMENT*/
 desp_acc_x_prev=desp_acc_x;
 desp_acc_y_prev=desp_acc_y;
 desp_acc_z_prev=desp_acc_z;

/*SET PREVIOUS ACCELERATION */
 ax_m_s2_prev=ax_m_s2;
 ay_m_s2_prev=ay_m_s2;
 az_m_s2_prev=az_m_s2;

/*SET PREVIOUS-1 ACCELERATION*/
 ax_m_s2_prev_o=ax_m_s2_prev;
 ay_m_s2_prev_o=ay_m_s2_prev;
 az_m_s2_prev_o=az_m_s2_prev;
 /*SET PREVIOUS TIME*/
 dt_prev=dt;
 v_x_prev=v_x_present;
 v_y_prev=v_y_present;
 v_z_prev=v_z_present;
 


/*
 * USE SERIAL TO SHOW DATA SEPARATED BY ; 
 */

  BlueSerial.print(ax_m_s2); 
  BlueSerial.print(";");
  BlueSerial.print(ay_m_s2);
  BlueSerial.print(";");
   BlueSerial.print(az_m_s2);
  BlueSerial.print(";");
 BlueSerial.print(desp_acc_y);
  BlueSerial.print(";");
 BlueSerial.println(desp_acc_z);


  delay(100);

  
}
        
/**********************************************************************
 *                      CALIBRATE SENSORS
 *                      8
 * FUNCTION USED TO CALIBRATE THE SENSORS                             
 * THIS SHOULD CALCULATE THE ACCELERATION AND ANGULAR VELOCITY OFFSETS
 *                                                                    
 * TYPE: VOID
 * PARAMETERS:NONE
 * RETURNS:NONE
 **********************************************************************/
 
void calibrate_sensors(){
  Serial.println("Calibrando...");
 for (int i=0; i<5; i++){
 /*
  * READ ACCELERATION AND ANGULAR VELOCITY
  */
  sensor.getAcceleration(&ax, &ay, &az);
  sensor.getRotation(&gx, &gy, &gz);

  /*
   * FILTER THE LECTURES
   */
  f_ax = f_ax-(f_ax>>5)+ax;
  p_ax = f_ax>>5;

  f_ay = f_ay-(f_ay>>5)+ay;
  p_ay = f_ay>>5;

  f_az = f_az-(f_az>>5)+az;
  p_az = f_az>>5;

  f_gx = f_gx-(f_gx>>3)+gx;
  p_gx = f_gx>>3;

  f_gy = f_gy-(f_gy>>3)+gy;
  p_gy = f_gy>>3;

  f_gz = f_gz-(f_gz>>3)+gz;
  p_gz = f_gz>>3;

  /*
   * EVERY 100 LECTURES CORRECT OFFSET
   */
  if (counter==100){

    /*
     * CALIBRATE ACCELEROMETER TO 1G AT Z ANGLE
     */
     
    if (p_ax>0) acc_x_off--;
    else {
      acc_x_off++;
      }
      
    if (p_ay>0) acc_y_off--;
    else {
      acc_y_off++;
      }
    if (p_az-16384>0) acc_z_off--;
    else {
      acc_z_off++;
      }
    /* SET ACCELERATION OFFSETS */
    sensor.setXAccelOffset(acc_x_off);
    sensor.setYAccelOffset(acc_y_off);
    sensor.setZAccelOffset(acc_z_off);

  /*
   * CALIBRATE GYROSCOPE
   */
    if (p_gx>0) gyro_x_off--;
    else {gyro_x_off++;}
    if (p_gy>0) gyro_y_off--;
    else {gyro_y_off++;}
    if (p_gz>0) gyro_z_off--;
    else {gyro_z_off++;}

    /* SET GYRO OFFSETS */
    
    sensor.setXGyroOffset(gyro_x_off);
    sensor.setYGyroOffset(gyro_y_off);
    sensor.setZGyroOffset(gyro_z_off);    

    counter=0;
  Serial.print("Counter:"+counter);
  }
  counter++;
  
 }
 Serial.println("Calibracion OK");
 int ready=1;
}

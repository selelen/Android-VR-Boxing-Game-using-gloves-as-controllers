import logo from './logo.svg';
import './App.css';
import { DataGrid } from '@mui/x-data-grid';
import * as React from 'react';
import { useEffect } from 'react';
import TableComponent from './Components/TableComponent';
import Box from '@mui/material/Box';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import { color } from '@mui/system';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';

import SendIcon from '@mui/icons-material/Send';

//Head Cells for the table Header of the ranking table

const headCells = [
  {
    id: 'id',
    numeric: true,
    disablePadding: true,
    label: 'ID',
  },
  {
    id: 'user_id',
    numeric: false,
    disablePadding: false,
    label: 'Usuario',
  },
  {
    id: 'score',
    numeric: true,
    disablePadding: false,
    label: 'Puntuación',
  },

];

function App() {
  const [value, setValue] = React.useState(0);
  //labels for the tabs that will determinate the page that will show
  const labels = ["Inicio", "Ranking", "Descargas"]
  const [rowsData, setRowsData] = React.useState([""])
  const [usersData, setUsersData] = React.useState([""])
  //per default the user is not registered and the username and emails for the register form are corrects
  const [registered, setRegistered] = React.useState(false)
  const [usedUser, setUsedUser] = React.useState(false)
  const [usedEmail, setUsedEmail] = React.useState(false)


  //functions that works with the api

  //function to register a new user, we will pass the user, email and password
  async function registerUser(username, password, email) {

    var api_url = 'http://20.70.7.244:8000/players/create/' + username + '/' + email + '/' + password
    const dataResponse =
      await fetch(api_url);
  }

  //get the ranking data
  const getRankingData = async () => {

    var api_url = 'http://20.70.7.244:8000/players/api/ranking/'
    const dataResponse =
      await fetch(api_url);
    const data = await dataResponse.json();
    //the result will be our rows for the ranking table
    setRowsData(data)

  }

  //users data
  const getUsers = async () => {

    var api_url = 'http://20.70.7.244:8000/players/api/users/'
    const dataResponse =
      await fetch(api_url);
    const data = await dataResponse.json();

    //we will use the users in order to determinate the player's username and to make the player registration
    setUsersData(data)
  }

  //function to always update users and rows data
  useEffect(() => {
    // Update data
    getRankingData()
    getUsers()
  });

  //function to know if an element is in a list, we will pass the key and the value to find
  function isInList(valueToFind, key) {
    //since the functions to get the data could return a promise, first we need to verify that there has been no errors before
    //mapping the array or it will throw us an error
    if (usersData != null && usersData != [""] && usersData.length > 2) {
      var isInList = false
      usersData.map((user) => {
        if (user[key] == valueToFind) {
          isInList = true
        }
      })
    }
    return isInList
  }

  //function to change the tab value when we select another tab
  const handleChange = (event, newValue) => {
    console.log(newValue)
    setValue(newValue);

  };

  //function to handle the submited form
  const handleSubmit = (event) => {
    //first we get the data from the inputs
    var username = document.getElementById("username-field").value
    var password = document.getElementById("password-field").value
    var email = document.getElementById("email-field").value
    //validate the data
    if (username != null && username != "" && password != null && password != "" && email != null && password != "") {
      //we will use our predefined function in order to know it the user or the email are already in use,
      //if is the case, we will show the error in the corresponding field
      if (isInList(username, 'username')) {
        setUsedUser(true)
      } else {
        setUsedUser(false)
        if (isInList(email, 'email')) {
          setUsedEmail(true)
        }
        //if everything is correct we send the data to our register function and set the user registered state at true in order
        //to show our welcome message and let the user know that they registered without problems
        else {
          setUsedEmail(false)
          registerUser(username, password, email)
          setRegistered(true)
        }
      }

    }
  }




  return (
    <><div>
      <Box sx={{ marginRight: 0, bgcolor: 'background.paper', textAlgin: 'center' }}>
        <Typography variant="h3" align="center" color='#131313' fontFamily={"Times"} style={{ fontWeight: 'medium', fontVariant: "small-caps" }}>Metal Punch</Typography>
        <Divider sx={{ width: '40%', marginLeft: '30%', marginTop: 1, height: 6 }} />
        <Tabs value={value} onChange={handleChange} sx={{ marginTop: 3 }}>
          <Tab label="Inicio" />
          <Tab label="Ranking" />
          <Tab label="Registro" />
        </Tabs>
      </Box>

    </div>
      <div style={{ marginTop: '3%', marginBottom: '5%', backgroundColor: 'white', width: '80%', marginLeft: '10%', height: 300 }}
        className={value == 1 ? '' : 'hidden'}>

        <TableComponent rows={rowsData} users={usersData} headCells={headCells} />

      </div>
      <div style={{ width: '50%', marginTop: '8%', marginLeft: '25%', backgroundColor: 'white', borderRadius: 6 }}
        className={value == 0 ? '' : 'hidden'}>
        <Typography variant="body1" gutterBottom style={{ margin: '5%' }}>
          <br />
          ¡Bienvenidos a la página del juego Metal Punch!

      Se trata de un videojuego de boxeo Realidad Virtual para Android, totalmente gratuito.
      Podréis controlarlo gracias a unos guantes creados específicamente para el juego.

      Desde aquí podréis completar el proceso de registro, así como visualizar el ranking de los jugadores.
      
      Esta es una versión provisional, así que, en caso de que tengáis cualquier sugerencia o comentario, podéis contactar con nosotros a través del correo
       <strong style={{color:'blue'}}> oficinaciclos@iesabastos.es</strong>

    ¡Esperamos que os guste!
          <br />  <br />
        </Typography>
      </div>
      <div style={{ marginTop: '10%', marginBottom: '5%', backgroundColor: 'white', width: '50%', marginLeft: '25%', height: 380 }}
        className={value == 2 ? '' : 'hidden'}>
        <br />
        {registered ? <Typography variant="body2" sx={{ textAlign: 'center', fontSize: 20, marginTop: '10%' }}>
          <br />¡Gracias por registrarte en <strong>Metal Punch</strong>!
          <br /> <br />
          ¡Esperamos que disfrutes de la experiencia!<br /></Typography> :
          <>
            <Typography variant="h5" align="center">Complete el formulario de registro</Typography>
            <Divider sx={{ width: '60%', marginLeft: '20%', marginTop: '2%' }} />
            <br />
            <Box
              sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                '& > :not(style)': { m: 1 },

              }}
            >
              <TextField
                helperText=""
                id="username-field"
                label={usedUser ? "Usuario ya en uso" : "Usuario"}
                color={usedUser ? "error" : "primary"}
                sx={{ height: 30 }}
              />
              <br />

              <TextField
                helperText=""
                id="email-field"
                label={usedEmail ? "Email ya en uso" : "Email"}
                color={usedEmail ? "error" : "primary"}
                type="email"
                sx={{ height: 30 }}
              />
              <br />
              <TextField
                helperText=" "
                id="password-field"
                label="Contraseña"
                type="password"
                sx={{ height: 30 }}
              />
              <br /> <br />
            </Box>
            <Button variant="outlined" endIcon={<SendIcon />} sx={{ float: 'right', marginRight: '5%', height: 30 }} onClick={handleSubmit}>
              Registrarse
            </Button>
            <br />
          </>}
      </div>
    </>
  );
}

export default App;

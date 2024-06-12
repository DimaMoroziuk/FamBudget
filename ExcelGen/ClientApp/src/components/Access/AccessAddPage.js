import React, { useState, useEffect } from 'react';
import { useHistory, useParams  } from "react-router-dom";
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import CircularProgress from '@mui/material/CircularProgress';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';;
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import InputLabel from '@mui/material/InputLabel';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { AccessTypeArray } from '../../helper';

export default function AccessAddPage() {
    const [loading, setLoading] = useState(false);
    const [initialAccess, setInitialAccess] = useState({});

    const history = useHistory ();
    const { id } = useParams();

    useEffect(() => {
      if(id)
      {
        setLoading(true);
        fetch(`api/Access/GetAccess?id=${id}`).then(response => response.json())
          .then(data => {
            setInitialAccess(data);
            setLoading(false);
          });
      }
    }, [setInitialAccess]);

    const changeInput = (event) => {
      setInitialAccess({ ...initialAccess, [event.target.name]: event.target.value });
    };
  

    const handleSubmit = (event) => {
      event.preventDefault();
      const data = new FormData(event.currentTarget);
      setLoading(true);
      if(id){
        fetch(`api/Access/PutAccess?id=${id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            ...initialAccess,
            email: data.get('receiverEmail'),
            accessType: Number(data.get('accessTypeId')),
          })
        })
        .then(response => history.push("/accesses"));
      } else {
        fetch(`api/Access/PostAccess`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            email: data.get('receiverEmail'),
            accessType: Number(data.get('accessTypeId')),
          })
        })
        .then(response => history.push("/accesses"));
      }
    };
  
    return (
        <Container component="main" maxWidth="xs">
          <CssBaseline />
          {loading ?
      <CircularProgress /> : 
          <Box
            sx={{
              marginTop: 8,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
            }}
          >
            <Typography component="h1" variant="h5">
              {id ? "Edit the access" : "Add new access" }
            </Typography>
            <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
              <Grid container spacing={2}>
                <Grid item xs={12}>
                  <TextField
                    required
                    fullWidth
                    id="receiverEmail"
                    label="Email of access receiver"
                    name="receiverEmail"
                    value={initialAccess?.accessReciever?.email}
                    defaultValue={initialAccess?.accessReciever?.email ? initialAccess?.accessReciever?.email : ""}
                    onChange={changeInput}
                  />
                </Grid>
                <Grid item xs={12}>
                <InputLabel id="accessType-select-label">Access Type</InputLabel>
                <Select
                labelId="accessType-select-label"
                id="accessTypeId"
                name="accessTypeId"
                label="Access Type"
                fullWidth
                value={initialAccess?.accessType}
                // defaultValue={initialAccess?.categoryId ? initialAccess?.categoryId : ""}
                onChange={changeInput}
                >
                {AccessTypeArray.map(ac => <MenuItem value={ac.value}>{ac.typeName}</MenuItem>)}
                </Select>
                </Grid>
              </Grid>
              <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
              >
                {id ? "Edit" : "Add"}
              </Button>
            </Box>
          </Box>}
        </Container>
    );
}
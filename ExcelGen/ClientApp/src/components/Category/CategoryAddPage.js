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

export default function CategoryAddPage() {
    const [loading, setLoading] = useState(false);
    const [initialCategory, setInitialCategory] = useState({});

    const history = useHistory ();

    const { id } = useParams();
    useEffect(() => {
      if(id)
      {
        setLoading(true);
        fetch(`api/Category/GetCategory?id=${id}`).then(response => response.json())
          .then(data => {
            setInitialCategory(data);
            setLoading(false);
          });
      }
    }, [setInitialCategory]);

    const changeInput = (event) => {
      setInitialCategory({ ...initialCategory, [event.target.name]: event.target.value });
    };
  

    const handleSubmit = (event) => {
      event.preventDefault();
      const data = new FormData(event.currentTarget);
      setLoading(true);
      if(id){
        fetch(`api/Category/PutCategory?id=${id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            ...initialCategory,
            name: data.get('name'),
            description: data.get('description'),
          })
        })
        .then(response => history.push("/Categories"));
      } else {
        fetch(`api/Category/PostCategory`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            name: data.get('name'),
            description: data.get('description'),
          })
        })
        .then(response => history.push("/Categories"));
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
              {id ? "Edit the Category" : "Add new Category" }
            </Typography>
            <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
              <Grid container spacing={2}>
                <Grid item xs={12}>
                  <TextField
                    required
                    fullWidth
                    id="name"
                    label="Name"
                    name="name"
                    value={initialCategory?.name}
                    defaultValue={initialCategory?.name ? initialCategory?.name : ""}
                    onChange={changeInput}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    multiline 
                    rows={4}
                    required
                    fullWidth
                    name="description"
                    label="Description"
                    id="description"
                    value={initialCategory?.description}
                    defaultValue={initialCategory?.description ? initialCategory?.description : ""}
                    onChange={changeInput}
                  />
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
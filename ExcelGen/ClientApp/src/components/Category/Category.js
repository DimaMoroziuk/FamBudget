import React, { useState, useEffect } from 'react';
import { useHistory  } from "react-router-dom";
import Link from '@mui/material/Link';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import Button from '@mui/material/Button';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Typography from '@mui/material/Typography';
import { mapNumberToMonthValue, getFormattedDate } from '../../helper'


// try to disable used categories for deletion
export default function Category() {
    const [data, setData] = useState([]);
    const [deletedIds, setDeletedIds] = useState([]);
    const history = useHistory ();


    function deleteIncome(row) {
        const undeletedRows = [...data.filter(x => x.id !== row.id)];
        setData(undeletedRows);
        deletedIds.push(row.id);
        setDeletedIds([...deletedIds]);
    }
    
    function handleSave() {
      fetch(`api/Category/DeleteIncome`, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(deletedIds)
      })
      .then(response => setDeletedIds([]));
  }

    useEffect(() => {
        
        fetch(`api/Category/GetCategories`).then(response => response.json())
        .then(data => setData(data));
      },
    [setData]);

  return (
    <React.Fragment>
      <Typography component="h2" variant="h6" color="primary" gutterBottom>
      Categories
      </Typography>
      <Table size="medium">
        <TableHead>
          <TableRow>
            <TableCell>Name</TableCell>
            <TableCell>Description</TableCell>
            <TableCell></TableCell>
            <TableCell align="right"></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((row) => (
              <TableRow key={row.id}>
                <TableCell>{row.name}</TableCell>
                <TableCell>{row.description}</TableCell>
                <TableCell><Link color="primary" onClick={() => history.push(`/category/add/${row.id}`)} sx={{ mt: 3 }}>
          Edit
        </Link></TableCell>
                <TableCell align="right"><Link color="primary" onClick={() => deleteIncome(row)} sx={{ mt: 3 }}>
          Delete
        </Link></TableCell>
              </TableRow>
            )
          )}
        </TableBody>
      </Table>
      <Link color="primary" href="category/add/" sx={{ mt: 20 }}>
        Add new Category
      </Link>
              <Button
                onClick={handleSave}
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
              >
                Save
              </Button>
      
    </React.Fragment>
  );
}
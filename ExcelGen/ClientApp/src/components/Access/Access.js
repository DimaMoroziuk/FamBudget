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
import { getAccessTypeName } from '../../helper'


// try to disable used categories for deletion
export default function Access() {
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
      fetch(`api/Access/DeleteAccess`, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(deletedIds)
      })
      .then(response => setDeletedIds([]));
  }

    useEffect(() => {
        fetch(`api/Access/GetAccesses`).then(response => response.json())
        .then(data => setData(data));
      },
    [setData]);

  return (
    <React.Fragment>
      <Typography component="h2" variant="h6" color="primary" gutterBottom>
      Accesses given by your user
      </Typography>
      <Table size="medium">
        <TableHead>
          <TableRow>
            <TableCell>Assignee name</TableCell>
            <TableCell>Access Type</TableCell>
            <TableCell></TableCell>
            <TableCell align="right"></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((row) => (
              <TableRow key={row.id}>
                <TableCell>{row.accessReciever.userName}</TableCell>
                <TableCell>{getAccessTypeName(row.accessType)}</TableCell>
                <TableCell><Link color="primary" onClick={() => history.push(`/access/add/${row.id}`)} sx={{ mt: 3 }}>
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
      <Link color="primary" href="access/add/" sx={{ mt: 20 }}>
        Add new Access
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
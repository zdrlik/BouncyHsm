using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncyHsm.Models.StorageObjects;

public class StorageObjectsListDto
{
    public int TotalCount
    {
        get;
        set;
    }

    public List<StorageObjectInfoDto> Objects
    {
        get;
        set;
    }

    public StorageObjectsListDto()
    {
        this.Objects = default!;
    }
}
